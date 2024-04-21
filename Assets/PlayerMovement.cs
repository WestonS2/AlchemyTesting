using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public static Vector3 _moveDirection;
	
	[Header("Movement Variables")]
	[SerializeField] float walkSpeed;
	[SerializeField] float sprintMultiplier;
	/*
	[Space(2)]
	[SerializeField] float jumpForce;
	[SerializeField] float maxJumpTime;
	[SerializeField] float groundCheckDistance;
	*/
	[Header("Other")]
	public Transform playerBody;
	
	IEnumerator JumpRoutine;
	
	Rigidbody playerRB;
	
	float playerSpeed;
	float jumpTime;
	
	bool isGrounded;
	bool isJumping;
	
	void Start()
	{
		if(GetComponent<Rigidbody>() == null)
		{
			playerRB = gameObject.AddComponent<Rigidbody>();
			playerRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}
		else playerRB = gameObject.GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		Movement();
	}
	
	void FixedUpdate()
	{
		_moveDirection *= Time.deltaTime * 10;
		playerRB.AddForce(_moveDirection);
		_moveDirection = new Vector3(0, 0, 0);
	}
	
	void Movement()
	{
		// Sprint
		if(Input.GetKey(Controls.keyCoordination[Controls.sprintKey])) playerSpeed = walkSpeed * sprintMultiplier;
		else playerSpeed = walkSpeed;
		
		// Linear Movement
		if(Input.GetKey(Controls.keyCoordination[Controls.forwardMove]))
		{
			_moveDirection += playerBody.forward * playerSpeed;
		}
		else if(Input.GetKey(Controls.keyCoordination[Controls.backwardMove]))
		{
			_moveDirection += -playerBody.forward * playerSpeed;
		}
		
		// Strafe Movement
		if(Input.GetKey(Controls.keyCoordination[Controls.rightStrafe]))
		{
			_moveDirection += playerBody.right * playerSpeed;
		}
		else if(Input.GetKey(Controls.keyCoordination[Controls.leftStrafe]))
		{
			_moveDirection += -playerBody.right * playerSpeed;
		}
	}
}
