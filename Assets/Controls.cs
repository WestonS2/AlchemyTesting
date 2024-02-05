using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
	public KeyCode generalInteraction;
	
	[HideInInspector] public static bool isInteracting;
	
	void Update()
	{
		if(Input.GetKey(generalInteraction)) isInteracting = true;
		else isInteracting = false;
	}
}
