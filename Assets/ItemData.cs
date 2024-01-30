using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
	public enum ITEM {None, Limestone, Root, Berries, Coin}
	public ITEM Item;
	
	public List<ITEM> compatibleItems = new List<ITEM>();
	
	public bool IsRecipie(ITEM item1, ITEM item2)
	{
		int count = 0;
		ITEM[] itemArray = new ITEM[2];
		itemArray[0] = item1;
		itemArray[1] = item2;
		foreach(ITEM checkItem in itemArray)
		{
			foreach(ITEM item in compatibleItems)
			{
				if(checkItem == item)
				{
					count++;
					break;
				}
			}
		}
		
		if(count == 2) return true;
		else return false;
	}
	
	public ITEM Recipie(ITEM item1, ITEM item2)
	{
		#region Coint Recipie
		if(item1 == ITEM.Root && item2 == ITEM.Berries || item2 == ITEM.Root && item1 == ITEM.Berries) return ITEM.Coin;
		else if(item1 == ITEM.Root && item2 == ITEM.Limestone || item2 == ITEM.Root && item1 == ITEM.Limestone) return ITEM.Coin;
		else if(item1 == ITEM.Berries && item2 == ITEM.Limestone || item2 == ITEM.Berries && item1 == ITEM.Limestone) return ITEM.Coin;
		else return ITEM.None;
		#endregion
	}
}
