using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages Non Player Character Events
/// </summary>

public class NPC_Events : MonoBehaviour
{
	public static NPC_Events instance;
	
	public static List<GameObject> activeNPC = new List<GameObject>();
	public List<Transform> enterPathTargets = new List<Transform>();
	public List<Transform> exitPathTargets = new List<Transform>();
	
	[HideInInspector] public List<GameObject> enterPathOccupied = new List<GameObject>();
	[HideInInspector] public List<GameObject> exitPathOccupied = new List<GameObject>();
	
	[SerializeField] List<GameObject> npcModels = new List<GameObject>();
	[Space(5)]
	[Header("Event Parameters")]
	public static int eventsToday;
	public static float timeBetweenEvents;
	[Header("NPC Dialogue")]
	[SerializeField] List<TextAsset> healthPotionDialogue = new List<TextAsset>();
	[SerializeField] List<TextAsset> firePotionDialogue = new List<TextAsset>();
	[SerializeField] List<TextAsset> icePotionDialogue = new List<TextAsset>();
	[SerializeField] List<TextAsset> growthPotionDialogue = new List<TextAsset>();
	[SerializeField] List<TextAsset> luckPotionDialogue = new List<TextAsset>();
	
	[SerializeField] GameObject npcPrefab;
	[SerializeField] Transform enterPath;
	[SerializeField] Transform exitPath;
	[SerializeField] Transform spawnPoint;
	[SerializeField] Transform despawnPoint;
	
	IEnumerator eventTimingRoutine;
	
	List<List<TextAsset>> dialogueOptions = new List<List<TextAsset>>();
	
	bool spawnBuffer;
	
	void Awake()
	{
		if(instance == null || instance == this) instance = this;
		else Destroy(this.gameObject);
		
		spawnBuffer = false;
		
		enterPathTargets = new List<Transform>();
		foreach(Transform pathPoint in enterPath)
		{
			enterPathTargets.Add(pathPoint);
		}
		
		exitPathTargets = new List<Transform>();
		foreach(Transform pathPoint in exitPath)
		{
			exitPathTargets.Add(pathPoint);
		}
		
		enterPathOccupied = new List<GameObject>();
		foreach(Transform point in enterPathTargets)
		{
			enterPathOccupied.Add(null);
		}
		
		exitPathOccupied = new List<GameObject>();
		foreach(Transform point in exitPathTargets)
		{
			exitPathOccupied.Add(null);
		}
		
		dialogueOptions.Add(healthPotionDialogue);
		dialogueOptions.Add(firePotionDialogue);
		dialogueOptions.Add(icePotionDialogue);
		dialogueOptions.Add(growthPotionDialogue);
		dialogueOptions.Add(luckPotionDialogue);
	}
	
	void Update()
	{
		if(!SceneManager.instance.dayComplete && !spawnBuffer)
		{
			eventTimingRoutine = EventTiming();
			StartCoroutine(eventTimingRoutine);
		}
	}
	
	IEnumerator EventTiming()
	{
		spawnBuffer = true;
		SpawnNewNPC();
		eventsToday--;
		if(eventsToday <= 0)
		{
			yield return new WaitWhile(() => activeNPC[activeNPC.Count - 1].GetComponent<NPC>().served == false);
			SceneManager.instance.dayComplete = true;
			StopCoroutine(eventTimingRoutine);
		}
		yield return new WaitForSeconds(timeBetweenEvents);
		spawnBuffer = false;
	}
	
	/* Forces all npcs to get served, no pay, stops eventTimingRoutine, calls EndOfDay.
	public void ForceEndOfDay()
	{
		StopCoroutine()
	}*/

		
	void SpawnNewNPC()
	{
		GameObject newNPC = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
		GameObject randomModel = Instantiate(npcModels[Random.Range(0, npcModels.Count)], newNPC.transform.position, newNPC.transform.rotation);
		randomModel.transform.parent = newNPC.transform;
		activeNPC.Add(newNPC);
		
		List<TextAsset> randomDialogue = dialogueOptions[Random.Range(0, dialogueOptions.Count)];
		newNPC.GetComponent<NPC>().dialogue = randomDialogue[Random.Range(0, randomDialogue.Count)];
		switch(randomDialogue)
		{
			case var value when value == healthPotionDialogue:
				newNPC.GetComponent<NPC>().wantedItem = ItemData.ITEM.HealthPotion;
				break;
				
			case var value when value == firePotionDialogue:
				newNPC.GetComponent<NPC>().wantedItem = ItemData.ITEM.FirePotion;
				break;
				
			case var value when value == icePotionDialogue:
				newNPC.GetComponent<NPC>().wantedItem = ItemData.ITEM.IcePotion;
				break;
				
			case var value when value == growthPotionDialogue:
				newNPC.GetComponent<NPC>().wantedItem = ItemData.ITEM.GrowthPotion;
				break;
				
			case var value when value == luckPotionDialogue:
				newNPC.GetComponent<NPC>().wantedItem = ItemData.ITEM.LuckPotion;
				break;
				
			default:
				break;
		}
	}
	
	void OnDestroy()
	{
		foreach(GameObject npc in activeNPC)
		{
			Destroy(npc);
		}
	}
}
