using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls an individual Non Player Character (NPC)
/// </summary>

public class NPC : MonoBehaviour
{
	public enum NPCSTATE {Idle, FollowPath};
	public NPCSTATE NpcState;
	
	NPC_Events EventManager;
	
	public bool served;
		
	[HideInInspector] public ItemData.ITEM wantedItem;
	[HideInInspector] public TextAsset dialogue;
	public int targetPathPosition;
	
	[SerializeField] float npcSpeed;
	
	Rigidbody npcRB;
	
	Vector3 moveDirection;
	
	void Awake()
	{
		targetPathPosition = 0;
		
		served = false;
		
		npcRB = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		if(EventManager == null && NPC_Events.instance != null) 
			EventManager = NPC_Events.instance;
		
		switch(NpcState)
		{
			case NPCSTATE.Idle:
				Idle();
				break;
				
			case NPCSTATE.FollowPath:
				if(EventManager != null)
				{
					if(!served) FollowPath(EventManager.enterPathTargets);
					else FollowPath(EventManager.exitPathTargets);
				}
				break;
				
			default:
				NpcState = NPCSTATE.Idle;
				break;
		}
	}
	
	public bool Serve(ItemData.ITEM servedItem)
	{
		served = true;
		targetPathPosition = 0;
		NpcState = NPCSTATE.FollowPath;
		if(servedItem == wantedItem) return true;
		else return false;
	}
	
	void Idle()
	{
		/*if(!served && targetPathPosition <= NPC_Events.enterPathTargets.Count - 1 && (EventManager.enterPathOccupied[targetPathPosition] == null || EventManager.enterPathOccupied[targetPathPosition] == gameObject))
		{
			NpcState = NPCSTATE.FollowPath;
			return;
		}
		if(served && targetPathPosition <= NPC_Events.exitPathTargets.Count - 1 && (EventManager.exitPathOccupied[targetPathPosition] == null || EventManager.exitPathOccupied[targetPathPosition] == gameObject))
		{
			NpcState = NPCSTATE.FollowPath;
			return;
		}*/
		if(served) NpcState = NPCSTATE.FollowPath;
	}
	
	void FollowPath(List<Transform> pathTargets)
	{
		// Path Following Conditions
		if(targetPathPosition >= pathTargets.Count - 1 && !served)
		{
			NpcState = NPCSTATE.Idle;
			if(!served) ShopFront.instance.currentCustomer = gameObject;
			return;
		}
		if(targetPathPosition >= pathTargets.Count - 1 && served)
		{
			Destroy(gameObject);
		}
		
		if(!served && EventManager.enterPathOccupied[targetPathPosition] != null && EventManager.enterPathOccupied[targetPathPosition] != gameObject)
		{
			NpcState = NPCSTATE.Idle;
			return;
		}
		else if(!served && EventManager.enterPathOccupied[targetPathPosition] == null)
			EventManager.enterPathOccupied[targetPathPosition] = gameObject;
		
		else if(served && EventManager.exitPathOccupied[targetPathPosition] != null && EventManager.exitPathOccupied[targetPathPosition] != gameObject)
		{
			NpcState = NPCSTATE.Idle;
			return;
		}
		else if(served && EventManager.exitPathOccupied[targetPathPosition] == null)
			EventManager.exitPathOccupied[targetPathPosition] = gameObject;
			
		// Movement
		Vector3 moveDirection = (pathTargets[targetPathPosition].position - transform.position).normalized;
		moveDirection.y = 0;
		transform.position += moveDirection * npcSpeed * Time.deltaTime;
		
		if(Vector3.Distance(transform.position, pathTargets[targetPathPosition].position) < 0.2f)
		{
			if(!served) EventManager.enterPathOccupied[targetPathPosition] = null;
			else EventManager.exitPathOccupied[targetPathPosition] = null;
			
			if(targetPathPosition + 1 <= pathTargets.Count - 1)
				targetPathPosition++;
		}
	}
}
