using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	
	public enum PlayerState {FreeRoam, WorkMode, CupboardMode, ShopMode}
	public PlayerState playerState;
	
	public int playerCoins;
	
	public IDictionary<ItemData.ITEM, GameObject> itemPrefabs = new Dictionary<ItemData.ITEM, GameObject>();
	
	[Header("General Game Variables")]
	public float interactionDistance;
	[SerializeField] float itemCursorFollowSpeed;
	[Header("Key Game Objects")]
	public GameObject GUI;
	[SerializeField] ShopFront shopFront;
	[SerializeField] TextMeshProUGUI coinCounter;
	[Header("Item Prefabs")]
	[SerializeField] List<GameObject> itemObjects = new List<GameObject>();
	
	GameObject playerObject;
	GameObject cauldron;
	GameObject selectedItem;
	GameObject workCamera;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else Destroy(this.gameObject);
		
		DontDestroyOnLoad(this);
		
		//Assigning prefabs to item type
		foreach(GameObject itemObj in itemObjects)
		{
			itemPrefabs.Add(itemObj.GetComponent<ItemData>().Item, itemObj);
		}
	}
	
	void Update()
	{
		UpdateGUI();
		
		//Find Player Object
		if(playerObject == null) playerObject = GameObject.FindWithTag("Player");
		
		//Player State Management
		if(playerState == PlayerState.FreeRoam)
		{
			if(!playerObject.activeSelf) playerObject.SetActive(true);
			if(!GUI.activeSelf) GUI.SetActive(true);
			if(Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
			if(Cursor.visible) Cursor.visible = false;
			if(workCamera != null && workCamera.activeSelf) workCamera.SetActive(false);
			
			//General Interaction
			if(Controls.isInteracting)
			{
				if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionDistance))
				{
					if(hit.collider.gameObject.tag == "Cauldron")
					{
						cauldron = hit.collider.gameObject;
						hit.collider.gameObject.GetComponent<Cauldron>().ToggleInteraction();
						workCamera = hit.collider.gameObject.GetComponent<Cauldron>().workCamera;
						playerState = PlayerState.WorkMode;
					}
					
					if(hit.collider.gameObject.tag == "Cupboard")
					{
						hit.collider.gameObject.GetComponent<Cupboard>().ToggleCupboard();
					}
					
					if(hit.collider.gameObject.tag == "ShopFront" || hit.collider.gameObject.tag == "NPC")
					{
						shopFront.ToggleShopFront();
					}
					
					if(hit.collider.gameObject.tag == "Item" && hit.collider.gameObject.GetComponent<ItemData>().Item == ItemData.ITEM.Coin)
					{
						playerCoins++;
						Destroy(hit.collider.gameObject);
					}
				}
			}
		}
		else if(playerState == PlayerState.WorkMode)
		{
			if(playerObject.activeSelf) playerObject.SetActive(false);
			if(GUI.activeSelf) GUI.SetActive(false);
			if(Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
			if(!Cursor.visible) Cursor.visible = true;
			
			//Exit Work Mode
			if(Input.GetKeyDown(KeyCode.Escape)) playerState = PlayerState.FreeRoam;
			
			//Work Space Interaction
			if(Input.GetMouseButtonDown(0))
			{
				selectedItem = null;
				Ray mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
				if(Physics.Raycast(mousePosition.origin, mousePosition.direction, out RaycastHit hit, 10, LayerMask.GetMask("Interactables")))
				{
					if(hit.collider.gameObject.tag == "Item")
					{
						selectedItem = hit.collider.gameObject;
					}
				}
			}
			else if(Input.GetMouseButtonUp(0) && selectedItem != null)
			{
				selectedItem = null;
			}
			
			if(selectedItem != null)
			{
				Ray mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
				if(Physics.Raycast(mousePosition.origin, mousePosition.direction, out RaycastHit hit, 10, LayerMask.GetMask("WorkStation")))
				{
					selectedItem.GetComponent<Rigidbody>().AddForce((hit.point - selectedItem.transform.position) * itemCursorFollowSpeed);
				}
				
				if(Vector3.Distance(hit.point, selectedItem.transform.position) < 0.2f)
				{
					Vector3 clampedSpeed = new Vector3(0, 0, 0);
					clampedSpeed.x = Mathf.Clamp(selectedItem.GetComponent<Rigidbody>().velocity.x, 0, 2);
					clampedSpeed.y = Mathf.Clamp(selectedItem.GetComponent<Rigidbody>().velocity.y, 0, 2);
					clampedSpeed.z = Mathf.Clamp(selectedItem.GetComponent<Rigidbody>().velocity.z, 0, 2);
					selectedItem.GetComponent<Rigidbody>().velocity = clampedSpeed;
				}
			}
		}
		
		else if(playerState == PlayerState.CupboardMode || playerState == PlayerState.ShopMode)
		{
			if(playerObject.activeSelf) playerObject.SetActive(false);
			if(!GUI.activeSelf) GUI.SetActive(true);
			if(Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
			if(!Cursor.visible) Cursor.visible = true;
		}
	}
	
	void UpdateGUI()
	{
		//Coin Count
		coinCounter.SetText($"{playerCoins}");
	}
}
