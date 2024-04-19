using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	
	public enum GAMESTATE {Menu, InGame};
	public GAMESTATE GameState;
	
	public int playerCoins;
	
	public IDictionary<ItemData.ITEM, GameObject> itemPrefabs = new Dictionary<ItemData.ITEM, GameObject>();
	public IDictionary<ItemData.ITEM, Texture> itemIcons = new Dictionary<ItemData.ITEM, Texture>();
	
	//[Header("General Game Variables")]
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
			iconCount++;
		}
	}
	
	void Update()
	{
		// Testing Save System
		if(Input.GetKeyDown(KeyCode.Alpha1)) GameManager.instance.Load();
		
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
	
	public void NextDay()
	{
		dayIndex++;
		NPC_Events.eventsToday = eventsPerDay;
		NPC_Events.timeBetweenEvents = timeBetweenEvents;
		eventsPerDay += eventIncreasePerDay;
		timeBetweenEvents -= decreaseTimeBetweenSpawns;
	}
	
	void MenuBehaviour()
	{
		// Menu conditions
	}
	
	void InGameBehaviour()
	{
		// Game conditions
	}
	
	#region Save & Load
	public void Save()
	{
		SaveData data = new SaveData();
		
		data.coins = playerCoins;
		List<int> itemKeysConvert = new List<int>();
		List<int> itemValuesConvert = new List<int>();
		foreach(ItemData.ITEM item in Inventory.instance.storedItems.Keys)
		{
			itemKeysConvert.Add(SaveSystem.itemID[item]);
			itemValuesConvert.Add(Inventory.instance.storedItems[item]);
		}
		data.itemKeys = itemKeysConvert.ToArray();
		data.itemValues = itemValuesConvert.ToArray();
		
		data.day = dayIndex;
		data.eventsInDay = eventsPerDay;
		data.spawnTime = timeBetweenEvents;
		
		SaveSystem.SavePlayerData(data);
		print($"<size=15>Game Saved</size>\n{Application.persistentDataPath}/SaveData.json");
	}
	
	public bool Load()
	{
		SaveData data = SaveSystem.LoadPlayerData();
		if(data == null) return false;
		
		playerCoins = data.coins;
		Inventory.instance.storedItems = new Dictionary<ItemData.ITEM, int>();
		for(int i = 0; i < data.itemKeys.Length; i++)
		{
			foreach(ItemData.ITEM item in SaveSystem.itemID.Keys)
			{
				if(SaveSystem.itemID[item] == data.itemKeys[i])
				{
					Inventory.instance.storedItems.Add(item, data.itemValues[i]);
					break;
				}
				else continue;
			}
		}
		
		dayIndex = data.day;
		eventsPerDay = data.eventsInDay;
		timeBetweenEvents = data.spawnTime;
		
		SaveSystem.SavePlayerData(data);
		print($"<size=15>Save Loaded</size>\n{Application.persistentDataPath}/SaveData.json");
		return true;
	}
	#endregion
}
