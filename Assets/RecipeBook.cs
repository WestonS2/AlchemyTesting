using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBook : MonoBehaviour
{
	[SerializeField] GameObject recipeUI;
	
	bool isOpen;
	
	void Start()
	{
		isOpen = false;
		
		recipeUI.SetActive(false);
	}
	
	public void ToggleRecipeBook()
	{
		isOpen = !isOpen;
		
		recipeUI.SetActive(isOpen);
		if(isOpen) SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.Interact;
		else SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.FreeRoam;
	}
}
