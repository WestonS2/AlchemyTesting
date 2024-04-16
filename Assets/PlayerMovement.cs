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
		//GroundCheck();
		
		Movement();
	}
	
	void FixedUpdate()
	{
		_moveDirection *= Screen.width * Time.deltaTime;
		_moveDirection /= 1000;
		playerRB.AddForce(_moveDirection);
		_moveDirection = new Vector3(0, 0, 0);
	}
	
	void Movement()
	{
		// Sprint
		if(Input.GetKey(KeyCode.LeftShift)) playerSpeed = walkSpeed * sprintMultiplier;
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
		
		/*
		// Jumping
		if(Input.GetKeyDown(Controls.jumpKey) && !isJumping && isGrounded)
		{
			JumpRoutine = JumpTiming();
			StartCoroutine(JumpRoutine);
			isJumping = true;
		}
		else if(Input.GetKeyUp(Controls.jumpKey) && isJumping)
		{
			StopCoroutine(JumpRoutine);
			isJumping = false;
		}
		
		if(isJumping)
		{
			_moveDirection += playerBody.up * jumpForce;
		}
		print(isJumping);*/
	}
	
	/*
	void GroundCheck()
	{
		isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDistance, LayerMask.GetMask("Ground"));
	}
	
	IEnumerator JumpTiming()
	{
		print("Routine Start");
		yield return new WaitForSeconds(jumpTime);
		isJumping = false;
		print("Routine Stop");
	}
	
#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + (-transform.up * groundCheckDistance));
	}
#endif*/
}
