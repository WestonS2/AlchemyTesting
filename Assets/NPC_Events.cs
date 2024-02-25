using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Events : MonoBehaviour
{
	public static List<GameObject> activeNPC = new List<GameObject>();
	public static List<Transform> enterPathTargets = new List<Transform>();
	public static List<Transform> exitPathTargets = new List<Transform>();
	
	[SerializeField] List<GameObject> npcModels = new List<GameObject>();
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
	
	List<List<TextAsset>> dialogueOptions = new List<List<TextAsset>>();
	
	void Start()
	{
		foreach(Transform pathPoint in enterPath)
		{
			enterPathTargets.Add(pathPoint);
		}
		
		foreach(Transform pathPoint in exitPath)
		{
			exitPathTargets.Add(pathPoint);
		}
		
		dialogueOptions.Add(healthPotionDialogue);
		dialogueOptions.Add(firePotionDialogue);
		dialogueOptions.Add(icePotionDialogue);
		dialogueOptions.Add(growthPotionDialogue);
		dialogueOptions.Add(luckPotionDialogue);
		
		SpawnNewNPC();
	}
		
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
	
#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		for(int i = 0; i < enterPathTargets.Count; i++)
		{
			Gizmos.DrawWireSphere(enterPathTargets[i].position, 0.1f);
			if(i + 1 < enterPathTargets.Count)
			{
				Gizmos.DrawLine(enterPathTargets[i].position, enterPathTargets[i + 1].position);
			}
		}
		
		Gizmos.color = Color.red;
		for(int i = 0; i < exitPathTargets.Count; i++)
		{
			Gizmos.DrawWireSphere(exitPathTargets[i].position, 0.1f);
			if(i + 1 < exitPathTargets.Count)
			{
				Gizmos.DrawLine(exitPathTargets[i].position, exitPathTargets[i + 1].position);
			}
		}
	}
#endif
}
