using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlastFurnace : MonoBehaviour
{
	public static BlastFurnace instance;
	
	[SerializeField] Slider slider;
	[SerializeField] float timeToHeat;
	
	bool open;
	
	void Awake()
	{
		if(instance == null) instance = this;
		else Destroy(this.gameObject);
		
		open = false;
	}
	
	void Update()
	{
		
	}
	
	public void ToggleFurnace()
	{
		open = !open;
	}
}
