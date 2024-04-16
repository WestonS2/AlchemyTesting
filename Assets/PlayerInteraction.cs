using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
	public static PlayerInteraction instance;
	
	[Header("FreeMode")]
	public float interactionDistance;
	[Header("WorkMode")]
	[SerializeField] float itemCursorFollowSpeed;
	
	PlayerMovement movementScript;
	PlayerCamera cameraScript;
	
	GameObject selectedItem;
	
	void Awake()
	{
		if(instance == null) instance = this;
		else Destroy(this.gameObject);
		
		movementScript = GetComponent<PlayerMovement>();
		cameraScript = GetComponent<PlayerCamera>();
	}
	
	void Update()
	{
		switch(SceneManager.instance.PlayerState)
		{
			case SceneManager.PLAYERSTATE.WorkMode:
				WorkMode();
				break;
				
			default:
				FreeMode();
				break;
		}
	}
	
	void WorkMode()
	{
		//Work Space Interaction
		if(Input.GetMouseButtonDown(0))
		{
			selectedItem = null;
			Ray mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(mousePosition.origin, mousePosition.direction, out RaycastHit hit, 10, LayerMask.GetMask("Interactables")))
			{
				if(hit.collider.gameObject.tag == "Item")
				{
					selectedItem = hit.collider.gameObject;
				}
			}
		}
		else if(Input.GetMouseButtonUp(0) && selectedItem != null)
		{
			selectedItem = null;
		}
		
		if(selectedItem != null)
		{
			Ray mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(mousePosition.origin, mousePosition.direction, out RaycastHit hit, 10, LayerMask.GetMask("WorkStation")))
			{
				selectedItem.GetComponent<Rigidbody>().AddForce((hit.point - selectedItem.transform.position) * itemCursorFollowSpeed);
			}
			
			if(Vector3.Distance(hit.point, selectedItem.transform.position) < 0.2f)
			{
				Vector3 clampedSpeed = new Vector3(0, 0, 0);
				clampedSpeed.x = Mathf.Clamp(selectedItem.GetComponent<Rigidbody>().velocity.x, 0, 2);
				clampedSpeed.y = Mathf.Clamp(selectedItem.GetComponent<Rigidbody>().velocity.y, 0, 2);
				clampedSpeed.z = Mathf.Clamp(selectedItem.GetComponent<Rigidbody>().velocity.z, 0, 2);
				selectedItem.GetComponent<Rigidbody>().velocity = clampedSpeed;
			}
		}
	}
	
	void FreeMode()
	{
		Ray screenCentreRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
		
		//Interaction
		if(Input.GetKeyDown(Controls.keyCoordination[Controls.generalInteraction]))
		{
			if(Physics.Raycast(screenCentreRay.origin, screenCentreRay.direction, out RaycastHit interactionHit, interactionDistance, LayerMask.GetMask("Interactables")))
			{
				print("ray");
				if(interactionHit.collider.gameObject.tag == "Cauldron")
				{
					print("Cauldron");
					interactionHit.collider.gameObject.GetComponent<Cauldron>().ToggleInteraction();
					SceneManager.instance.workCamera = interactionHit.collider.gameObject.GetComponent<Cauldron>().workCamera;
					SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.WorkMode;
				}
				
				if(interactionHit.collider.gameObject.tag == "Cupboard")
				{
					interactionHit.collider.gameObject.GetComponent<Cupboard>().ToggleCupboard();
					SceneManager.instance.workCamera = interactionHit.collider.gameObject.GetComponent<Cupboard>().cupboardCamera;
					SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.WorkMode;
				}
				
				if(interactionHit.collider.gameObject.tag == "ShopFront" || interactionHit.collider.gameObject.tag == "NPC")
				{
					ShopFront.instance.ToggleShopFront();
					SceneManager.instance.workCamera = ShopFront.instance.shopCamera;
					SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.WorkMode;
				}
				
				if(interactionHit.collider.gameObject.tag == "MortarPestle")
				{
					SceneManager.instance.workCamera = interactionHit.collider.gameObject.GetComponent<MortarPestle>().workCamera;
					SceneManager.instance.PlayerState = SceneManager.PLAYERSTATE.WorkMode;
				}
			}
		}
		//Pickup Item
		if(Input.GetMouseButtonDown(Controls.pickUpMouseKey))
		{
			if(Physics.Raycast(screenCentreRay.origin, screenCentreRay.direction, out RaycastHit pickupHit, interactionDistance))
			{
				if(pickupHit.collider.gameObject.tag == "Item" && pickupHit.collider.gameObject.GetComponent<ItemData>().Item == ItemData.ITEM.Coin)
				{
					GameManager.instance.playerCoins++;
					Destroy(pickupHit.collider.gameObject);
				}
				
				if(pickupHit.collider.gameObject.tag == "Item" && pickupHit.collider.gameObject.GetComponent<ItemData>().Item != ItemData.ITEM.Coin)
				{
					Inventory.instance.AddItem(pickupHit.collider.gameObject.GetComponent<ItemData>().Item, 1);
					Destroy(pickupHit.collider.gameObject);
				}
			}
		}
		//Inventory
		if(Input.GetKeyDown(Controls.inventoryKey))
		{
			bool opened = Inventory.instance.ToggleInventory();
			if(opened)
			{
				movementScript.enabled = false;
				cameraScript.enabled = false;
			}
			else
			{
				movementScript.enabled = true;
				cameraScript.enabled = true;
			}
		}
	}
}
