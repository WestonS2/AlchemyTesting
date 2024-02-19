using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cupboard : MonoBehaviour
{
	[SerializeField] GameObject cupboardUI;
	[SerializeField] GameObject cupboardCamera;
	
	bool cupboardOpen;
	
	void Start()
	{
		if(cupboardUI.activeSelf) cupboardUI.SetActive(false);
		if(cupboardCamera.activeSelf) cupboardCamera.SetActive(false);
		cupboardOpen = false;
	}
	
	void Update()
	{
		if(cupboardOpen && Controls.isExiting)
		{
			ToggleCupboard();
		}
	}
	
	public void ToggleCupboard()
	{
		cupboardOpen = !cupboardOpen;
		if(cupboardOpen)
		{
			if(GameManager.instance.playerState != GameManager.PlayerState.CupboardMode)
			{
				GameManager.instance.playerState = GameManager.PlayerState.CupboardMode;
			}
		}
		else
		{
			if(GameManager.instance.playerState != GameManager.PlayerState.FreeRoam)
			{
				GameManager.instance.playerState = GameManager.PlayerState.FreeRoam;
			}
		}
		
		cupboardUI.SetActive(cupboardOpen);
		cupboardCamera.SetActive(cupboardOpen);
	}
	
	public void AddItemToInventory(ItemData.ITEM item)
	{
		//To be combined with inventory script
		print($"Item Added: {item}");
	}
}
