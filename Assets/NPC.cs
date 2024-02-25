using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	public enum NPCSTATE {Idle, FollowPath, Customer};
	public NPCSTATE NpcState;
	
	[HideInInspector] public ItemData.ITEM wantedItem;
	[HideInInspector] public TextAsset dialogue;
	[HideInInspector] public int pathPosition;
	
	[SerializeField] float npcSpeed;
	
	Rigidbody npcRB;
	
	Vector3 moveDirection;
	
	bool served;
	
	void Awake()
	{
		pathPosition = 0;
		
		served = false;
		
		transform.parent = NPC_Events.enterPathTargets[0];
		
		npcRB = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		switch(NpcState)
		{
			case NPCSTATE.Idle:
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
	
	void FixedUpdate()
	{
		//npcRB.velocity = moveDirection;
	}
	
	public bool Serve(ItemData.ITEM servedItem)
	{
		served = true;
		NpcState = NPCSTATE.FollowPath;
		if(servedItem == wantedItem) return true;
		else return false;
	}
	
	void FollowPath(List<Transform> pathTargets)
	{
		foreach(GameObject npc in NPC_Events.activeNPC)
		{
			if(npc != this.gameObject && npc.GetComponent<NPC>().pathPosition == pathPosition)
			{
				return;
			}
		}
		
		if(transform.parent != pathTargets[pathPosition])
		{
			transform.parent = pathTargets[pathPosition];
		}
		
		if(Vector3.Distance(pathTargets[pathPosition].position, transform.position) < 0.3f)
		{
			if(pathPosition >= pathTargets.Count - 1)
			{
				NpcState = NPCSTATE.Customer;
				ShopFront.currentCustomer = this.gameObject;
				return;
			}
			else pathPosition++;
		}
		
		moveDirection = (pathTargets[pathPosition].position - transform.position).normalized * npcSpeed * Time.deltaTime;
		moveDirection.y = 0;
		transform.position += moveDirection;
	}
}
