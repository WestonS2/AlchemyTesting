using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
	public static KeyCode generalInteraction = KeyCode.E;
	public static KeyCode exit = KeyCode.Escape;
	
	[HideInInspector] public static bool isInteracting;
	[HideInInspector] public static bool isExiting;
	
	void Update()
	{
		if(Input.GetKey(generalInteraction)) isInteracting = true;
		else isInteracting = false;
		
		if(Input.GetKey(exit)) isExiting = true;
		else isExiting = false;
	}
}
