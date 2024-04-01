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
		if(cupboardOpen && Input.GetKeyDown(Controls.keyCoordination[Controls.exitKey]))
		{
			ToggleCupboard();
		}
	}
	
	public void ToggleCupboard()
	{
		cupboardOpen = !cupboardOpen;
		if(cupboardOpen)
		{
			if(GameManager.instance.PlayerState != GameManager.PLAYERSTATE.CupboardMode)
			{
				GameManager.instance.PlayerState = GameManager.PLAYERSTATE.CupboardMode;
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}
		else
		{
			if(GameManager.instance.PlayerState != GameManager.PLAYERSTATE.FreeRoam)
			{
				GameManager.instance.PlayerState = GameManager.PLAYERSTATE.FreeRoam;
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		
		cupboardUI.SetActive(cupboardOpen);
		cupboardCamera.SetActive(cupboardOpen);
	}
}
