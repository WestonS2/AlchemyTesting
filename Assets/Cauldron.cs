using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the functionality of the cauldren and its recipies
/// </summary>

public class Cauldron : MonoBehaviour
{
	public GameObject workCamera;
	[SerializeField] Transform potionSpawnLocation;
	
	public List<GameObject> itemsInCauldron = new List<GameObject>();
	public List<GameObject> workSpaceItems = new List<GameObject>();
	
	bool workingMode;
	bool spawnBuffer;
	
	public void ToggleInteraction()
	{
		workCamera.SetActive(!workCamera.activeSelf);
		workingMode = workCamera.activeSelf;
	}
	
	void Awake()
	{
		workCamera.SetActive(false);
		workingMode = false;
		spawnBuffer = false;
	}
	
	void Update()
	{
		if(itemsInCauldron.Count == 3)
		{
			if(CheckItemCompatibility(itemsInCauldron[0], itemsInCauldron[1], itemsInCauldron[2]))
			{
				ItemData.ITEM recipieProduct = itemsInCauldron[0].GetComponent<ItemData>().Recipie(itemsInCauldron[1].GetComponent<ItemData>().Item, itemsInCauldron[2].GetComponent<ItemData>().Item);
				if(recipieProduct != ItemData.ITEM.None)
				{
					SpawnNewPotion(recipieProduct);
				}
				else
				{
					//print("Cauldron Product Failure");
					print(recipieProduct);
				}
			}
		}
	}
	
	bool CheckItemCompatibility(GameObject item1, GameObject item2, GameObject item3)
	{
		return item1.GetComponent<ItemData>().IsRecipie(item2.GetComponent<ItemData>().Item, item3.GetComponent<ItemData>().Item);
	}
	
	void SpawnNewPotion(ItemData.ITEM potion)
	{
		Instantiate(GameManager.instance.itemPrefabs[potion], potionSpawnLocation.position, GameManager.instance.itemPrefabs[potion].transform.rotation);
		foreach(GameObject ingr in itemsInCauldron)
		{
			Destroy(ingr);
		}
		itemsInCauldron = new List<GameObject>();
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(workingMode)
		{
			if(col.gameObject.tag == "Item")
			{
				itemsInCauldron.Add(col.gameObject);
				workSpaceItems.Remove(col.gameObject);
				col.gameObject.SetActive(false);
			}
		}
	}
	
	public void ResetIngredients()
	{
		if(!spawnBuffer) StartCoroutine(SpawnBuffer());
	}
	
	IEnumerator SpawnBuffer()
	{
		spawnBuffer = true;
		foreach(GameObject ingredient in itemsInCauldron)
		{
			ingredient.transform.position = potionSpawnLocation.position;
			ingredient.SetActive(true);
			yield return new WaitForSeconds(0.5f);
		}
		foreach(GameObject ingredient in workSpaceItems)
		{
			ingredient.transform.position = potionSpawnLocation.position;
			ingredient.SetActive(true);
			yield return new WaitForSeconds(0.5f);
		}
		itemsInCauldron = new List<GameObject>();
		spawnBuffer = false;
	}
}
