using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
	public static IDictionary<ItemData.ITEM, int> itemID = new Dictionary<ItemData.ITEM, int>()
	{
		{ItemData.ITEM.Rose, 1},
		{ItemData.ITEM.Charcoal, 2},
		{ItemData.ITEM.Berries, 3},
		{ItemData.ITEM.Sulphur, 4},
		{ItemData.ITEM.Coal, 5},
		{ItemData.ITEM.CoalDust, 6},
		{ItemData.ITEM.FrogSlime, 7},
		{ItemData.ITEM.Lavender, 8},
		{ItemData.ITEM.Daisy, 9},
		{ItemData.ITEM.Bone, 10},
		{ItemData.ITEM.Wheat, 11},
		{ItemData.ITEM.Egg, 12},
		{ItemData.ITEM.Coin, 13},
		{ItemData.ITEM.HealthPotion, 14},
		{ItemData.ITEM.FirePotion, 15},
		{ItemData.ITEM.IcePotion, 16},
		{ItemData.ITEM.GrowthPotion, 17},
		{ItemData.ITEM.LuckPotion, 18}
	};
	
	public static void SavePlayerData(SaveData data)
	{
		string path = Path.Combine(Application.persistentDataPath + "/SaveData.json");
		
		if(File.Exists(path)) File.Delete(path);
		
		string jsonData = JsonUtility.ToJson(data, true);
		
		using(StreamWriter streamFile = File.CreateText(path))
		{
			streamFile.WriteLine(jsonData);
			streamFile.Close();
		}
	}
}
