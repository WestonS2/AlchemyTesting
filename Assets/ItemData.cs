using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
	public enum ITEM
	{
		None, 
		Rose, 
		Charcoal, 
		Berries, 
		Sulphur, 
		Coal, 
		CoalDust, 
		FrogSlime, 
		Lavender, 
		Daisy, 
		Bone, 
		Wheat, 
		Egg, 
		Coin, 
		HealthPotion, 
		FirePotion, 
		IcePotion, 
		GrowthPotion, 
		LuckPotion
	}
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
		#region Health Potion Recipie
		if(item1 == ITEM.Rose && item2 == ITEM.Rose && Item == ITEM.Berries) return ITEM.HealthPotion;
		else if((item1 == ITEM.Rose && item2 == ITEM.Berries || item1 == ITEM.Berries && item2 == ITEM.Rose) && Item == ITEM.Rose) return ITEM.HealthPotion;
		#endregion
		
		#region Fire Potion Recipie
		else if(item1 == ITEM.Sulphur && item2 == ITEM.Sulphur && Item == ITEM.Charcoal) return ITEM.FirePotion;
		else if((item1 == ITEM.Sulphur && item2 == ITEM.Charcoal || item1 == ITEM.Charcoal && item2 == ITEM.Sulphur) && Item == ITEM.Sulphur) return ITEM.FirePotion;
		#endregion
		
		#region Ice Potion Recipie
		else if((item1 == ITEM.CoalDust && item2 == ITEM.FrogSlime || item1 == ITEM.FrogSlime && item2 == ITEM.CoalDust) && Item == ITEM.Lavender) return ITEM.IcePotion;
		else if((item1 == ITEM.Lavender && item2 == ITEM.FrogSlime || item1 == ITEM.FrogSlime && item2 == ITEM.Lavender) && Item == ITEM.CoalDust) return ITEM.IcePotion;
		else if((item1 == ITEM.CoalDust && item2 == ITEM.Lavender || item1 == ITEM.Lavender && item2 == ITEM.CoalDust) && Item == ITEM.FrogSlime) return ITEM.IcePotion;
		#endregion
		
		#region Growth Potion Recipie
		else if((item1 == ITEM.Egg && item2 == ITEM.Wheat || item1 == ITEM.Wheat && item2 == ITEM.Egg) && Item == ITEM.Bone) return ITEM.GrowthPotion;
		else if((item1 == ITEM.Bone && item2 == ITEM.Wheat || item1 == ITEM.Wheat && item2 == ITEM.Bone) && Item == ITEM.Egg) return ITEM.GrowthPotion;
		else if((item1 == ITEM.Egg && item2 == ITEM.Bone || item1 == ITEM.Bone && item2 == ITEM.Egg) && Item == ITEM.FrogSlime) return ITEM.GrowthPotion;
		#endregion
		
		#region Luck Potion Recipie
		else if((item1 == ITEM.Coin && item2 == ITEM.Daisy || item1 == ITEM.Daisy && item2 == ITEM.Coin) && Item == ITEM.FrogSlime) return ITEM.LuckPotion;
		else if((item1 == ITEM.FrogSlime && item2 == ITEM.Daisy || item1 == ITEM.Daisy && item2 == ITEM.FrogSlime) && Item == ITEM.Coin) return ITEM.LuckPotion;
		else if((item1 == ITEM.Coin && item2 == ITEM.FrogSlime || item1 == ITEM.FrogSlime && item2 == ITEM.Coin) && Item == ITEM.Daisy) return ITEM.LuckPotion;
		#endregion
		
		else return ITEM.None;
	}
}
