using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Variables")]
	[SerializeField] float walkSpeed;
	[SerializeField] float sprintMultiplier;
	[Header("Other")]
	
	Rigidbody playerRB;
	Vector3 moveDirection;
	
	float playerSpeed;
	
	float horizontal;
	float vertical;
	
	void Start()
	{
		playerRB = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		
		if(Input.GetKey(KeyCode.LeftShift)) playerSpeed = walkSpeed * sprintMultiplier;
		else playerSpeed = walkSpeed;
		moveDirection = Camera.main.transform.forward * vertical * playerSpeed + Camera.main.transform.right * horizontal * playerSpeed;
		moveDirection.y = 0;
	}
	
	void FixedUpdate()
	{
		playerRB.AddForce(moveDirection * Time.deltaTime);
	}
}
