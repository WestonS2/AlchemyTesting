using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
	public ItemData.ITEM item;
	[SerializeField] int itemCost;
	[SerializeField] TextMeshProUGUI itemValue;
	[SerializeField] Shop shopScript;
	
	void Awake()
	{
		itemValue.SetText($"Coins : {itemCost}");
	}
	
	public void BuyItem()
	{
		if(GameManager.instance.playerCoins >= itemCost)
		{
			GameManager.instance.playerCoins -= itemCost;
			shopScript.AddItemToInventory(item);
		}
	}
}
