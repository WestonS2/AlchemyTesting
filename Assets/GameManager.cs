using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	
	public enum GAMESTATE {Menu, InGame};
	public GAMESTATE GameState;
	
	public enum PLAYERSTATE {FreeRoam, WorkMode, CupboardMode, ShopMode, Inventory};
	public PLAYERSTATE PlayerState;
	
	public int playerCoins;
	
	public IDictionary<ItemData.ITEM, GameObject> itemPrefabs = new Dictionary<ItemData.ITEM, GameObject>();
	public IDictionary<ItemData.ITEM, Texture> itemIcons = new Dictionary<ItemData.ITEM, Texture>();
	
	//[Header("General Game Variables")]
	[Header("Key Game Objects")]
	public GameObject GUI;
	[SerializeField] GameObject crosshairUI;
	[SerializeField] ShopFront shopFront;
	[SerializeField] TextMeshProUGUI coinCounter;
	[Header("NPC Events")]
	public int dayIndex;
	[SerializeField] int eventIncreasePerDay;
	[SerializeField] float decreaseTimeBetweenSpawns;
	[SerializeField] private int eventsPerDay;
	[SerializeField] private float timeBetweenEvents;
	[Header("Item Prefabs")]
	[SerializeField] List<GameObject> itemObjects = new List<GameObject>();
	[Header("Item Icons")]
	[SerializeField] List<Texture> itemImages = new List<Texture>();
	
	//Player objects
	GameObject playerObject;
	Transform playerCamera;
	Transform playerBody;
	
	[HideInInspector] public GameObject workCamera;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else Destroy(this.gameObject);
		
		DontDestroyOnLoad(this);
		
		//Assigning prefabs to item type
		int iconCount = 0;
		foreach(GameObject itemObj in itemObjects)
		{
			itemPrefabs.Add(itemObj.GetComponent<ItemData>().Item, itemObj);
			if(itemObj.GetComponent<ItemData>().Item != ItemData.ITEM.Coin)
				itemIcons.Add(itemObj.GetComponent<ItemData>().Item, itemImages[iconCount]);
			if(iconCount + 1 >= itemImages.Count) iconCount++;
		}
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1)) GameManager.instance.Save();
		switch(GameState)
		{
			case GAMESTATE.Menu:
				MenuBehaviour();
				break;
				
			case GAMESTATE.InGame:
				InGameBehaviour();
				break;
				
			default:
				print("Critical Game State Error!");
				break;
		}
	}
	
	void MenuBehaviour()
	{
		// Menu conditions
	}
	
	void InGameBehaviour()
	{
		UpdateGUI();
		
		LocatePlayer();
		
		#region Player State
		switch(PlayerState)
		{
			case PLAYERSTATE.FreeRoam:
				FreeRoam();
				break;
				
			case PLAYERSTATE.Inventory:
				InventoryInteract();
				break;
				
			case PLAYERSTATE.WorkMode:
				WorkMode();
				break;
				
			case PLAYERSTATE.CupboardMode | PLAYERSTATE.ShopMode:
				Interaction();
				break;
				
			default:
				break;
		}
		#endregion
	}
	
	public void NextDay()
	{
		dayIndex++;
		NPC_Events.eventsPerDay = eventsPerDay;
		NPC_Events.timeBetweenEvents = timeBetweenEvents;
		eventsPerDay += eventIncreasePerDay;
		timeBetweenEvents -= decreaseTimeBetweenSpawns;
	}
	
	void LocatePlayer()
	{
		if(playerObject == null)
		{
			playerObject = GameObject.FindWithTag("Player");
			playerCamera = playerObject.transform.GetChild(0);
			playerBody = playerObject.transform.GetChild(1);
		}
	}
	
	void FreeRoam()
	{
		if(!playerObject.GetComponent<PlayerMovement>().enabled) playerObject.GetComponent<PlayerMovement>().enabled = true;
		if(!playerObject.GetComponent<PlayerCamera>().enabled) playerObject.GetComponent<PlayerCamera>().enabled = true;
		if(!playerCamera.gameObject.activeSelf) playerCamera.gameObject.SetActive(true);
		if(!playerBody.gameObject.activeSelf) playerBody.gameObject.SetActive(true);
		if(!GUI.activeSelf) GUI.SetActive(true);
		if(!crosshairUI.activeSelf) crosshairUI.SetActive(true);
		if(Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
		if(Cursor.visible) Cursor.visible = false;
		if(workCamera != null && workCamera.activeSelf) workCamera.SetActive(false);
	}
	
	void WorkMode()
	{
		if(playerObject.GetComponent<PlayerMovement>().enabled) playerObject.GetComponent<PlayerMovement>().enabled = false;
		if(playerObject.GetComponent<PlayerCamera>().enabled) playerObject.GetComponent<PlayerCamera>().enabled = false;
		if(playerCamera.gameObject.activeSelf) playerCamera.gameObject.SetActive(false);
		if(playerBody.gameObject.activeSelf) playerBody.gameObject.SetActive(false);
		if(!GUI.activeSelf) GUI.SetActive(true);
		if(crosshairUI.activeSelf) crosshairUI.SetActive(false);
		if(Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
		if(!Cursor.visible) Cursor.visible = true;
	}
	
	void InventoryInteract()
	{
		if(playerObject.GetComponent<PlayerMovement>().enabled) playerObject.GetComponent<PlayerMovement>().enabled = false;
		if(playerObject.GetComponent<PlayerCamera>().enabled) playerObject.GetComponent<PlayerCamera>().enabled = false;
		if(!playerCamera.gameObject.activeSelf) playerCamera.gameObject.SetActive(true);
		if(!playerBody.gameObject.activeSelf) playerBody.gameObject.SetActive(true);
		if(!GUI.activeSelf) GUI.SetActive(true);
		if(crosshairUI.activeSelf) crosshairUI.SetActive(false);
		if(Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
		if(!Cursor.visible) Cursor.visible = true;
	}
	
	void Interaction()
	{
		if(playerObject.GetComponent<PlayerMovement>().enabled) playerObject.GetComponent<PlayerMovement>().enabled = false;
		if(playerObject.GetComponent<PlayerCamera>().enabled) playerObject.GetComponent<PlayerCamera>().enabled = false;
		if(playerCamera.gameObject.activeSelf) playerCamera.gameObject.SetActive(false);
		if(playerBody.gameObject.activeSelf) playerBody.gameObject.SetActive(false);
		if(!GUI.activeSelf) GUI.SetActive(true);
		if(crosshairUI.activeSelf) crosshairUI.SetActive(false);
		if(Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
		if(!Cursor.visible) Cursor.visible = true;
	}
	
	void UpdateGUI()
	{
		//Coin Count
		coinCounter.SetText($"{playerCoins}");
	}
	
	#region Save & Load
	public void Save()
	{
		SaveData data = new SaveData();
		
		data.coins = playerCoins;
		//if(Inventory.instance != null)
		data.items = Inventory.instance.storedItems;
		data.day = dayIndex;
		data.eventsInDay = eventsPerDay;
		data.spawnTime = timeBetweenEvents;
		
		SaveSystem.SavePlayerData(data);
		print($"<size=15>Game Saved</size>\n{Application.persistentDataPath}/SaveData.json");
	}
	#endregion
}
