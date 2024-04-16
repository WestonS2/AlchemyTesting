using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cupboard : MonoBehaviour
{
	[SerializeField] GameObject cupboardUI;
	public GameObject cupboardCamera;
	
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
			if(SceneManager.instance.PlayerState != SceneManager.PLAYERSTATE.WorkMode)
				SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.WorkMode;

		}
		else
		{
			if(SceneManager.instance.PlayerState != SceneManager.PLAYERSTATE.FreeRoam)
				SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.FreeRoam;
		}
		
		cupboardUI.SetActive(cupboardOpen);
		cupboardCamera.SetActive(cupboardOpen);
	}
}
