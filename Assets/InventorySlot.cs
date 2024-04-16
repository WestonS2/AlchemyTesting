using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
	public void DropItemInv()
	{
		if(MortarPestle.isOpen) Inventory.instance.MoveInventoryItem(transform.GetSiblingIndex());
		else Inventory.instance.DropInventoryItem(transform.GetSiblingIndex());
	}
}
