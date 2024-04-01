using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
	// Player
	public int coins;
	public IDictionary<ItemData.ITEM, int> items;
	// Game
	public int day;
	public int eventsInDay;
	public float spawnTime;
}
