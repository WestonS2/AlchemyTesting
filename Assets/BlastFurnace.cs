using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the functionality of the Mortar & Pestle
/// </summary>

public class BlastFurnace : MonoBehaviour
{
	public static BlastFurnace instance;
	
	[HideInInspector] public static ItemData.ITEM itemInFurnace;
	[HideInInspector] public static int itemAmount; 
	[HideInInspector] public static bool isWorking;
	[HideInInspector] public static bool isOpen;
	
	public GameObject workCamera;
	[SerializeField] Transform itemDropPoint;
	[SerializeField] Slider workSlider;
	[SerializeField] GameObject startButton;
	[SerializeField] GameObject resetButton;
	[Space(10)]
	[Tooltip("Time it takes to heat an object")]
	[SerializeField] float workTime;
	[Tooltip("Time between item drops")]
	[SerializeField] float dropItemBuffer;
	
	float sliderValue;
	
	void Awake()
	{
		if(instance == null) instance = this;
		else Destroy(this.gameObject);
	}
	
	void Start()
	{
		isWorking = false;
		
		startButton.SetActive(false);
		
		workCamera.SetActive(false);
	}
	
	void Update()
	{
		if(Input.GetKeyDown(Controls.exitKey))
			SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.FreeRoam;
		
		if(isWorking) WorkTimer();
		
		if(itemInFurnace == ItemData.ITEM.Coal) startButton.SetActive(true);
		else startButton.SetActive(false);
		
		if(itemInFurnace != ItemData.ITEM.None && !isWorking) resetButton.SetActive(true);
		else resetButton.SetActive(false);
		
		if(workCamera.activeSelf)
		{
			isOpen = true;
			if(Input.GetKeyDown(Controls.inventoryKey))
				Inventory.instance.ToggleInventory();
		}
		else isOpen = false;
	}
	
	void WorkTimer()
	{
		workSlider.value = sliderValue;
		sliderValue += 1 * Time.deltaTime;
		if(sliderValue >= workTime)
		{
			DropBuffer(ItemData.ITEM.Charcoal, itemAmount);
			StopFurnace();
		}
	}
	
	public void StartFurnace()
	{
		isWorking = true;
		workSlider.gameObject.SetActive(true);
		startButton.SetActive(false);
		sliderValue = 0;
	}
	
	void StopFurnace()
	{
		isWorking = false;
		workSlider.gameObject.SetActive(false);
	}
	
	public bool AddItem(ItemData.ITEM item)
	{
		print("Item Added Furnace");
		if(itemInFurnace != item) return false;
		else if(itemInFurnace == item)
		{
			itemAmount++;
			return true;
		}
		else
		{
			itemInFurnace = item;
			itemAmount = 1;
			return true;
		}
	}
	
	public bool RemoveItem(ItemData.ITEM item)
	{
		if(itemInFurnace != item) return false;
		else if(itemInFurnace == item && itemAmount > 1)
		{
			itemAmount--;
			return true;
		}
		else
		{
			itemInFurnace = ItemData.ITEM.None;
			itemAmount = 0;
			return true;
		}
	}
	
	public void ResetItems()
	{
		if(itemAmount > 0)
		{
			DropItem(itemAmount);
		}
	}
	
	public void DropItem(int dropAmount)
	{
		if(dropAmount > itemAmount) return;
		StartCoroutine(DropBuffer(itemInFurnace, dropAmount));
		for(int i = 0; i < dropAmount; i++)
		{
			RemoveItem(itemInFurnace);
		}
	}
	
	IEnumerator DropBuffer(ItemData.ITEM item, int amount)
	{
		for(int i = 0; i < amount; i++)
		{
			Instantiate(GameManager.instance.itemPrefabs[item], itemDropPoint.position, itemDropPoint.rotation);
			yield return new WaitForSeconds(dropItemBuffer);
		}
	}
}
