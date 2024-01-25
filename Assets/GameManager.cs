using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	
	public enum PlayerState {FreeRoam, WorkMode}
	public PlayerState playerState;
	
	[Header("General Game Variables")]
	[SerializeField] float interactionDistance;
	[SerializeField] float itemCursorFollowSpeed;
	[Header("Key Game Objects")]
	public GameObject GUI;
	
	GameObject playerObject;
	GameObject workingEquipment;
	GameObject selectedItem;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else Destroy(this.gameObject);
		
		DontDestroyOnLoad(this);
	}
	
	void Update()
	{
		//Find Player Object
		if(playerObject == null) playerObject = GameObject.FindWithTag("Player");
		
		//Player State Management
		if(playerState == PlayerState.FreeRoam)
		{
			if(!playerObject.activeSelf) playerObject.SetActive(true);
			if(!GUI.activeSelf) GUI.SetActive(true);
			if(Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
			if(Cursor.visible) Cursor.visible = false;
		}
		else if(playerState == PlayerState.WorkMode)
		{
			if(playerObject.activeSelf) playerObject.SetActive(false);
			if(GUI.activeSelf) GUI.SetActive(false);
			if(Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
			if(!Cursor.visible) Cursor.visible = true;
		}
		
		//Equimpent Interaction
		if(Input.GetKeyDown(KeyCode.E) && playerState == PlayerState.FreeRoam)
		{
			if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionDistance))
			{
				if(hit.collider.gameObject.tag == "Equipment")
				{
					workingEquipment = hit.collider.gameObject;
					hit.collider.gameObject.GetComponent<Equipment>().ToggleInteraction();
					playerState = PlayerState.WorkMode;
				}
			}
		}
		
		//Item Interaction
		if(Input.GetMouseButtonDown(0) && playerState == PlayerState.WorkMode)
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
		else if(Input.GetMouseButtonUp(0) && playerState == PlayerState.WorkMode && selectedItem != null)
		{
			selectedItem = null;
		}
		
		if(selectedItem != null && playerState == PlayerState.WorkMode)
		{
			Ray mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(mousePosition.origin, mousePosition.direction, out RaycastHit hit, 10, LayerMask.GetMask("WorkStation")))
			{
				selectedItem.GetComponent<Rigidbody>().AddForce((hit.point - selectedItem.transform.position) * itemCursorFollowSpeed);
			}
			
			if(Vector3.Distance(hit.point, selectedItem.transform.position) < 0.3f)
			{
				Vector3 clampedSpeed = new Vector3(0, 0, 0);
				clampedSpeed.x = Mathf.Clamp(selectedItem.GetComponent<Rigidbody>().velocity.x, 0, 2);
				clampedSpeed.y = Mathf.Clamp(selectedItem.GetComponent<Rigidbody>().velocity.y, 0, 2);
				clampedSpeed.z = Mathf.Clamp(selectedItem.GetComponent<Rigidbody>().velocity.z, 0, 2);
				selectedItem.GetComponent<Rigidbody>().velocity = clampedSpeed;
			}
		}
	}
}
