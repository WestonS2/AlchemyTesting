using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopFront : MonoBehaviour
{
	IDictionary<ItemData.ITEM, int> potionValue = new Dictionary<ItemData.ITEM, int>();
	
	[SerializeField] static GameObject shopCamera;
	[SerializeField] GameObject shopUI;
	[SerializeField] Transform itemSpawnLocation;
	
	static bool shopOpen;
	
	void Start()
	{
		shopCamera = transform.GetChild(0).gameObject;
		shopCamera.SetActive(false);
		
		shopOpen = false;
		
		potionValue.Add(ItemData.ITEM.HealthPotion, 6);
		potionValue.Add(ItemData.ITEM.FirePotion, 7);
		potionValue.Add(ItemData.ITEM.IcePotion, 7);
		potionValue.Add(ItemData.ITEM.GrowthPotion, 9);
		potionValue.Add(ItemData.ITEM.LuckPotion, 15);
	}
	
	public static void ToggleShopFront()
	{
		shopOpen = !shopOpen;
		
		if(shopOpen)
		{
			GameManager.instance.playerState = GameManager.PlayerState.CupboardMode;
			shopCamera.SetActive(true);
		}
		else
		{
			shopCamera.SetActive(false);
			GameManager.instance.playerState = GameManager.PlayerState.CupboardMode;
		}
	}
	
	public void ToggleInventory()
	{
		//Inventory script stuff here
	}
	
	void Pay(bool correctItem, ItemData.ITEM item)
	{
		if(correctItem)
		{
			for(int i = 0; i < potionValue[item]; i++)
			{
				StartCoroutine(ItemSpawnBuffer(item));
			}
		}
		else
		{
			for(int i = 0; i < (potionValue[item] / 2); i++)
			{
				StartCoroutine(ItemSpawnBuffer(item));
			}
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Item")
		{
			Pay(NPC_Events.enterPathTargets[NPC_Events.enterPathTargets.Count - 1].GetChild(0).gameObject.GetComponent<NPC>().Serve(col.gameObject.GetComponent<ItemData>().Item), col.gameObject.GetComponent<ItemData>().Item);
		}
	}
	
	IEnumerator ItemSpawnBuffer(ItemData.ITEM spawnItem)
	{
		yield return new WaitForSeconds(0.15f);
		Instantiate(GameManager.instance.itemPrefabs[spawnItem], itemSpawnLocation.position, itemSpawnLocation.rotation);
	}
}
