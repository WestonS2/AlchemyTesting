using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
	[SerializeField] GameObject shopUI;
	[SerializeField] GameObject shopCamera;
	
	bool shopOpen;
	
	void Start()
	{
		if(shopUI.activeSelf) shopUI.SetActive(false);
		if(shopCamera.activeSelf) shopCamera.SetActive(false);
		shopOpen = false;
	}
	
	void Update()
	{
		if(shopOpen && Controls.isExiting)
		{
			ToggleShop();
		}
	}
	
	public void ToggleShop()
	{
		shopOpen = !shopOpen;
		if(shopOpen)
		{
			if(GameManager.instance.playerState != GameManager.PlayerState.ShopMode)
			{
				GameManager.instance.playerState = GameManager.PlayerState.ShopMode;
			}
		}
		else
		{
			if(GameManager.instance.playerState != GameManager.PlayerState.FreeRoam)
			{
				GameManager.instance.playerState = GameManager.PlayerState.FreeRoam;
			}
		}
		
		shopUI.SetActive(shopOpen);
		shopCamera.SetActive(shopOpen);
	}
	
	public void AddItemToInventory(ItemData.ITEM item)
	{
		//To be combined with inventory script
		print($"Item Added: {item}");
	}
}
