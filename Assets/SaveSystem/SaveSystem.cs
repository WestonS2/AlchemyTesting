using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
	public static string savefilePath = Path.Combine(Application.persistentDataPath + "/SaveData.json");
	
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
		if(File.Exists(savefilePath)) File.Delete(savefilePath);
		
		string jsonData = JsonUtility.ToJson(data, true);
		
		using(StreamWriter streamFile = File.CreateText(savefilePath))
		{
			streamFile.WriteLine(jsonData);
			streamFile.Close();
		}
	}
	
	public static SaveData LoadPlayerData()
	{
		if(!File.Exists(savefilePath)) return null;
		
		string readData = File.ReadAllText(savefilePath);
		
		return JsonUtility.FromJson<SaveData>(readData);
	}
	
	public static void DeleteSaveFile()
	{
		if(File.Exists(savefilePath)) File.Delete(savefilePath);
	}
}
