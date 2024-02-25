using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
	public static KeyCode generalInteraction = KeyCode.E;
	public static KeyCode exit = KeyCode.Escape;
	
	public static int pickUpMouseKey = 0;
	
	public static bool isInteracting;
	public static bool pickingUp;
	public static bool isExiting;
	
	void Update()
	{
		if(Input.GetKey(generalInteraction)) isInteracting = true;
		else isInteracting = false;
		
		if(Input.GetMouseButton(pickUpMouseKey)) pickingUp = true;
		else pickingUp = false;
		
		if(Input.GetKey(exit)) isExiting = true;
		else isExiting = false;
	}
}
