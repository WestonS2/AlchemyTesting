using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public static Vector3 _moveDirection;
	
	[Header("Movement Variables")]
	[SerializeField] float walkSpeed;
	[SerializeField] float sprintMultiplier;
	[Header("Other")]
	public Transform playerBody;
	
	Rigidbody playerRB;
	
	float playerSpeed;
	
	void Start()
	{
		playerRB = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		Movement();
	}
	
	void FixedUpdate()
	{
		playerRB.AddForce(_moveDirection);
		_moveDirection = new Vector3(0, 0, 0);
	}
	
	void Movement()
	{
		if(Input.GetKey(KeyCode.LeftShift)) playerSpeed = walkSpeed * sprintMultiplier;
		else playerSpeed = walkSpeed;
		
		#region Linear Movement
		if(Input.GetKey(Controls.keyCoordination[Controls.forwardMove]))
		{
			_moveDirection += playerBody.forward * playerSpeed;
		}
		else if(Input.GetKey(Controls.keyCoordination[Controls.backwardMove]))
		{
			_moveDirection += -playerBody.forward * playerSpeed;
		}
		#endregion
		#region Strafe Movement
		if(Input.GetKey(Controls.keyCoordination[Controls.rightStrafe]))
		{
			_moveDirection += playerBody.right * playerSpeed;
		}
		else if(Input.GetKey(Controls.keyCoordination[Controls.leftStrafe]))
		{
			_moveDirection += -playerBody.right * playerSpeed;
		}
		#endregion
		
		_moveDirection.y = 0;
		_moveDirection *= Time.deltaTime * 100;
	}
}
