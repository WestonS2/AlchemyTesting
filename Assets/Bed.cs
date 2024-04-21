using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bed : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI bedMessage;
	
	void Start()
	{
		bedMessage.gameObject.SetActive(false);
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			if(SceneManager.instance.dayComplete)
			bedMessage.SetText("End day?\nE to Interact");
			else bedMessage.SetText("End day early?\nE to Interact");
			bedMessage.gameObject.SetActive(true);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player")
			bedMessage.gameObject.SetActive(false);
	}
}
