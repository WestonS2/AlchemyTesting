using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	public enum NPCSTATE {Idle, FollowPath, Customer};
	public NPCSTATE NpcState;
	
	public bool served;
	
	List<bool> positionFree = new List<bool>();
		
	[HideInInspector] public ItemData.ITEM wantedItem;
	[HideInInspector] public TextAsset dialogue;
	[HideInInspector] public int pathPosition;
	
	[SerializeField] float npcSpeed;
	
	Rigidbody npcRB;
	
	Vector3 moveDirection;
	
	void Awake()
	{
		pathPosition = 0;
		
		served = false;
		
		npcRB = GetComponent<Rigidbody>();
		
		foreach(Transform point in NPC_Events.enterPathTargets)
		{
			positionFree.Add(false);
		}
	}
	
	void Update()
	{
		switch(NpcState)
		{
			case NPCSTATE.Idle:
				Idle();
				break;
				
			case NPCSTATE.FollowPath:
				if(!served) FollowPath(NPC_Events.enterPathTargets);
				else FollowPath(NPC_Events.exitPathTargets);
				break;
				
			case NPCSTATE.Customer:
				break;
				
			default:
				break;
		}
	}
	
	public bool Serve(ItemData.ITEM servedItem)
	{
		served = true;
		NpcState = NPCSTATE.FollowPath;
		if(servedItem == wantedItem) return true;
		else return false;
	}
	
	void Idle()
	{
		if(!positionFree[pathPosition])
		{
			NpcState = NPCSTATE.FollowPath;
			return;
		}
	}
	
	void FollowPath(List<Transform> pathTargets)
	{
		if(positionFree[pathPosition + 1])
		{
			NpcState = NPCSTATE.Idle;
			return;
		}
		
		if(Vector3.Distance(pathTargets[pathPosition].position, transform.position) < 0.2f)
		{
			if(pathPosition >= pathTargets.Count - 1)
			{
				NpcState = NPCSTATE.Customer;
				ShopFront.currentCustomer = this.gameObject;
				return;
			}
			else
			{
				positionFree[pathPosition] = false;
				pathPosition++;
				positionFree[pathPosition] = true;
			}
		}
		
		moveDirection = (pathTargets[pathPosition].position - transform.position).normalized * npcSpeed * Time.deltaTime;
		moveDirection.y = 0;
		transform.position += moveDirection;
	}
}
