using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[SerializeField] float sensitivity;
	
	Vector3 camRotation;
	Quaternion newRotation;
	
	float mouseX;
	float mouseY;
	
	void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
	void Update()
	{
		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");
		
		camRotation = new Vector3(-mouseY, mouseX, 0);
		newRotation.eulerAngles = Camera.main.transform.eulerAngles + camRotation;
		Camera.main.transform.rotation = newRotation;
	}
}
