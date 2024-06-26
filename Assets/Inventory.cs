using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
	public static Inventory instance;
	
	[HideInInspector] public Vector3 itemDropPoint;
	
	public IDictionary<ItemData.ITEM, int> storedItems = new Dictionary<ItemData.ITEM, int>();
	
	[SerializeField] GameObject inventoryUI;
	[SerializeField] Transform defaultDropLocation;
	[SerializeField] float dropBufferTime;
	Transform itemSlots;
	GameObject dropButtonText;
	
	int inventorySlotCount = 10;
	
	void Awake()
	{
		if(instance == null || instance == this) instance = this;
		else Destroy(this.gameObject);
		
		inventoryUI.SetActive(false);
		
		itemSlots = inventoryUI.transform.GetChild(0);
		dropButtonText = inventoryUI.transform.GetChild(1).gameObject;
		UpdateInventoryUI();
	}
	
	#region Inventory Interaction
	public bool ToggleInventory()
	{
		inventoryUI.SetActive(!inventoryUI.activeSelf);
		if(inventoryUI.activeSelf)
		{
			UpdateInventoryUI();
			if(SceneManager.instance.PlayerState == SceneManager.PLAYERSTATE.FreeRoam)
			{
				SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.Interact;
				itemDropPoint = defaultDropLocation.position;
				print("Inv item drop point reset");
			}
			itemSlots.gameObject.SetActive(true);
			dropButtonText.SetActive(true);
		}
		else
		{
			if(SceneManager.instance.PlayerState == SceneManager.PLAYERSTATE.Interact)
				SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.FreeRoam;
		}
		return inventoryUI.activeSelf;
	}
	
	public bool AddItem(ItemData.ITEM item, int amount)
	{
		if(storedItems.Count >= inventorySlotCount) return false;
		
		foreach(ItemData.ITEM checkItem in storedItems.Keys)
		{
			if(checkItem == item)
			{
				storedItems[item] += amount;
				UpdateInventoryUI();
				return true;
			}
		}
		
		storedItems.Add(item, amount);
		UpdateInventoryUI();
		return true;
	}
	
	public bool RemoveItem(ItemData.ITEM item, int amount)
	{
		foreach(ItemData.ITEM checkItem in storedItems.Keys)
		{
			if(checkItem == item)
			{
				if(storedItems[item] - amount <= 0)
				{
					storedItems.Remove(item);
					UpdateInventoryUI();
					return true;
				}
				else
				{
					storedItems[item] -= amount;
					UpdateInventoryUI();
					return true;
				}
			}
		}
		return false;
	}
	
	public bool DropItem(ItemData.ITEM item, int amount)
	{
		foreach(ItemData.ITEM storedItem in storedItems.Keys)
		{
			if(storedItem == item)
			{
				if(storedItems[item] - amount <= 0)
				{
					StartCoroutine(DropBuffer(itemDropPoint, item, amount, dropBufferTime));
					storedItems.Remove(item);
					UpdateInventoryUI();
					return true;
				}
				else
				{
					StartCoroutine(DropBuffer(itemDropPoint, item, amount, dropBufferTime));
					storedItems[item] -= amount;
					UpdateInventoryUI();
					return true;
				}
			}
		}
		return false;
	}
	
	public void MoveInventoryItem(int slot)
	{
		int itemCount = 0;
		foreach(ItemData.ITEM item in storedItems.Keys)
		{
			if(itemCount == slot)
			{
				if(MortarPestle.isOpen)
				{
					if(storedItems[item] - 1 <= 0)
					{
						MortarPestle.instance.AddItem(item);
						storedItems.Remove(item);
					}
					else
					{
						MortarPestle.instance.AddItem(item);
						storedItems[item] -= 1;
					}
				}
				else if(BlastFurnace.isOpen)
				{
					if(storedItems[item] - 1 <= 0)
					{
						BlastFurnace.instance.AddItem(item);
						storedItems.Remove(item);
					}
					else
					{
						BlastFurnace.instance.AddItem(item);
						storedItems[item] -= 1;
					}
				}
				else DropItem(item, 1);
				break;
			}
			itemCount += 1;
		}
		UpdateInventoryUI();
	}
	
	public void DropInventoryItem(int slot)
	{
		int itemCount = 0;
		foreach(ItemData.ITEM item in storedItems.Keys)
		{
			if(itemCount == slot)
			{
				DropItem(item, 1);
				break;
			}
			itemCount += 1;
		}
		UpdateInventoryUI();
	}
	#endregion
	
	IEnumerator DropBuffer(Vector3 dropPosition, ItemData.ITEM item, int amount, float bufferTime)
	{
		for(int i = amount; i > 0; i--)
		{
			Instantiate(GameManager.instance.itemPrefabs[item], dropPosition, Quaternion.Euler(0, 0, 0));
			yield return new WaitForSeconds(bufferTime);
		}
	}
	
	void UpdateInventoryUI()
	{
		//Reset Slots
		foreach(Transform slot in itemSlots.transform)
		{
			slot.GetChild(0).gameObject.SetActive(false);
			slot.GetChild(0).GetChild(0).gameObject.SetActive(false);
			slot.GetChild(1).gameObject.SetActive(false);
		}
		//Update Stored Items
		foreach(ItemData.ITEM item in storedItems.Keys)
		{
			foreach(Transform slot in itemSlots.transform)
			{
				if(!slot.GetChild(0).gameObject.activeSelf)
				{
					slot.GetChild(0).gameObject.SetActive(true);
					slot.GetChild(1).gameObject.SetActive(true);
					slot.GetChild(0).gameObject.GetComponent<RawImage>().texture = GameManager.instance.itemIcons[item];
					slot.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().SetText(storedItems[item].ToString());
					break;
				}
				else continue;
			}
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(defaultDropLocation.position, 0.05f);
	}
}
