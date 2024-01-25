using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
	[SerializeField] GameObject workCamera;
	
	List<GameObject> itemsInEquipment = new List<GameObject>();
	
	bool workingMode;
	
	int whiteCount;
	int blueCount;
	int redCount;
	int brownCount;
	
	void Awake()
	{
		workCamera.SetActive(false);
		workingMode = false;
		
		whiteCount = 0;
		blueCount = 0;
		redCount = 0;
		brownCount = 0;
	}
	
	void Update()
	{
		if(blueCount == 1 && redCount == 1 && brownCount == 1)
		{
			Destroy(this.gameObject);
		}
	}
	
	public void ToggleInteraction()
	{
		workCamera.SetActive(!workCamera.activeSelf);
		workingMode = workCamera.activeSelf;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(workingMode)
		{
			if(col.gameObject.tag == "Item")
			{
				if(col.gameObject.GetComponent<Item>().itemData.ingredientType == ItemDatabase.IngredientType.White)
				{
					whiteCount++;
					itemsInEquipment.Add(col.gameObject);
					col.gameObject.SetActive(false);
				}
				else if(col.gameObject.GetComponent<Item>().itemData.ingredientType == ItemDatabase.IngredientType.Blue)
				{
					blueCount++;
					itemsInEquipment.Add(col.gameObject);
					col.gameObject.SetActive(false);
				}
				else if(col.gameObject.GetComponent<Item>().itemData.ingredientType == ItemDatabase.IngredientType.Red)
				{
					redCount++;
					itemsInEquipment.Add(col.gameObject);
					col.gameObject.SetActive(false);
				}
				else if(col.gameObject.GetComponent<Item>().itemData.ingredientType == ItemDatabase.IngredientType.Brown)
				{
					brownCount++;
					itemsInEquipment.Add(col.gameObject);
					col.gameObject.SetActive(false);
				}
			}
		}
	}
}
