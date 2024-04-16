using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
	// Player
	public int coins;
	public int[] itemKeys;
	public int[] itemValues;
	// Game
	public int day;
	public int eventsInDay;
	public float spawnTime;
}
