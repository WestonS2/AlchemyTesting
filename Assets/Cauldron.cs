using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the functionality of the cauldren and its recipies
/// </summary>

public class Cauldron : MonoBehaviour
{
	public static Cauldron instance;
	
	public GameObject workCamera;
	[SerializeField] Transform spawnLocation;
	[SerializeField] Slider brewSlider;
	[SerializeField] GameObject brewButton;
	[SerializeField] GameObject resetButton;
	
	public List<GameObject> itemsInCauldron = new List<GameObject>();
	public List<GameObject> workSpaceItems = new List<GameObject>();
	
	ItemData.ITEM recipieProduct;
	
	float brewTimer;
	
	bool workingMode;
	bool spawnBuffer;
	bool brewReady;
	bool brewing;
	
	public void ToggleInteraction()
	{
		workCamera.SetActive(!workCamera.activeSelf);
		workingMode = workCamera.activeSelf;
		if(workingMode)
		{
			Inventory.instance.itemDropPoint = spawnLocation.position;
		}
	}
	
	public void BrewPotion()
	{
		if(brewReady && !brewing) StartCoroutine(BrewTiming());
	}
	
	public void ResetIngredients()
	{
		if(brewing) return;
		resetButton.SetActive(false);
		if(!spawnBuffer) StartCoroutine(SpawnBuffer());
	}
	
	void Awake()
	{
		if(instance == null) instance = this;
		else Destroy(this.gameObject);
		
		workCamera.SetActive(false);
		workingMode = false;
		spawnBuffer = false;
		brewReady = false;
		brewing = false;
	}
	
	void Update()
	{
		#region Inputs
		//Exit Work Mode
		if(Input.GetKeyDown(KeyCode.Escape) && workingMode)
		{
			ToggleInteraction();
			SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.FreeRoam;
		}
		
		//Toggle Inventory
		if(Input.GetKeyDown(KeyCode.Tab) && workingMode)
		{
			Inventory.instance.itemDropPoint = spawnLocation.position;
			Inventory.instance.ToggleInventory();
		}
		#endregion
		
		Brewing();
	}
	
	void Brewing()
	{
		BrewCheck();
		
		if(brewReady && !brewing)
			brewButton.SetActive(true);
		else 
			brewButton.SetActive(false);
		
		if(!brewing && itemsInCauldron.Count > 0)
			resetButton.SetActive(true);
		else 
			resetButton.SetActive(false);
		
		if(brewing)
		{
			brewSlider.gameObject.SetActive(true);
			brewSlider.value = 1 - (brewTimer / ItemData.brewTime[recipieProduct]);
			brewTimer -= 1 * Time.deltaTime;
		}
		else brewSlider.gameObject.SetActive(false);
	}
	
	IEnumerator BrewTiming()
	{
		brewing = true;
		brewTimer = ItemData.brewTime[recipieProduct];
		yield return new WaitWhile(() => brewTimer > 0);
		SpawnNewPotion(recipieProduct);
		brewing = false;
	}
	
	void BrewCheck()
	{
		if(itemsInCauldron.Count == 3)
		{
			if(CheckItemCompatibility(itemsInCauldron[0], itemsInCauldron[1], itemsInCauldron[2]))
			{
				recipieProduct = itemsInCauldron[0].GetComponent<ItemData>().Recipie(itemsInCauldron[1].GetComponent<ItemData>().Item, itemsInCauldron[2].GetComponent<ItemData>().Item);
				if(recipieProduct != ItemData.ITEM.None)
				{
					brewReady = true;
				}
				else
				{
					brewReady = false;
					//print("Cauldron Product Failure: " + recipieProduct);
				}
			}
			else brewReady = false;
		}
		else brewReady = false;
	}
	
	bool CheckItemCompatibility(GameObject item1, GameObject item2, GameObject item3)
	{
		return item1.GetComponent<ItemData>().IsRecipie(item2.GetComponent<ItemData>().Item, item3.GetComponent<ItemData>().Item);
	}
	
	void SpawnNewPotion(ItemData.ITEM potion)
	{
		Instantiate(GameManager.instance.itemPrefabs[potion], spawnLocation.position, GameManager.instance.itemPrefabs[potion].transform.rotation);
		foreach(GameObject ingr in itemsInCauldron)
		{
			Destroy(ingr);
		}
		itemsInCauldron = new List<GameObject>();
	}
	
	IEnumerator SpawnBuffer()
	{
		spawnBuffer = true;
		/*foreach(GameObject ingredient in workSpaceItems)
		{
			ingredient.transform.position = spawnLocation.position;
			ingredient.SetActive(true);
			yield return new WaitForSeconds(0.5f);
		}*/
		foreach(GameObject ingredient in itemsInCauldron)
		{
			ingredient.transform.position = spawnLocation.position;
			ingredient.SetActive(true);
			workSpaceItems.Add(ingredient);
			yield return new WaitForSeconds(0.5f);
		}
		itemsInCauldron = new List<GameObject>();
		spawnBuffer = false;
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
}
