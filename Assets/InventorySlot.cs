using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
	public void DropItemInv()
	{
		Inventory.instance.DropInventoryItem(transform.GetSiblingIndex());
	}
}
