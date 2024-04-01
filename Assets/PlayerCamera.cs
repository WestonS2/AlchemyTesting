using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[SerializeField] float sensitivity;
	
	PlayerMovement movementScript;
	
	Vector3 camRotation;
	Quaternion newRotation;
	
	float mouseX;
	float mouseY;
	
	void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
		movementScript = GetComponent<PlayerMovement>();
	}
	
	void Update()
	{
		mouseX += Input.GetAxis("Mouse X") * sensitivity;
		mouseY += Input.GetAxis("Mouse Y") * sensitivity;
		mouseY = Mathf.Clamp(mouseY, -70, 70);
		
		camRotation = new Vector3(-mouseY, mouseX, 0);
		movementScript.playerBody.rotation = Quaternion.Euler(0, mouseX, 0);
		Camera.main.transform.eulerAngles = camRotation;
	}
}
