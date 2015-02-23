using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float turnSmoothing = 15f;
	public float walkDampTime = 5f;
	public float runDampTime = 10f;
	public float characterSpeed;
	public float topRunSpeed = 6f;
	public float topWalkSpeed = 3f;
	public float terminalVelocity = 10f;
	public float fallingSpeed;
	public float gravity = 0.2f;
	public Vector3 targetDirection;
	public float rotSpeed = 3f;
	public Vector3 inputVector = Vector3.zero;

	private int idleState;
	private int dyingState;
	private int locomotionState;
	private int jumpingState;
	private int attackingState;
	
	private int attackingBool;
	private int smashingBool;
	private int runningBool;
	private int defendingBool;
	private int jumpingBool;
	
	private int speedFloat;
	
	private int MotionLockedHash;

	private Animator animator;
	private CharacterController controller;

	void Awake()
	{
		//if (networkView.isMine) 
		//{
			animator = GetComponent<Animator>();
			controller = GetComponent<CharacterController>();
			idleState = Animator.StringToHash("Base Layer.Idle");
			dyingState = Animator.StringToHash("Base Layer.Dying");
			locomotionState = Animator.StringToHash("Base Layer.Locomotion");
			jumpingState = Animator.StringToHash("Base Layer.Jumping");
			attackingState = Animator.StringToHash("Base Layer.Attacking");
			
			attackingBool = Animator.StringToHash("Attacking");
			smashingBool = Animator.StringToHash("Smashing");
			runningBool = Animator.StringToHash("Running");
			defendingBool = Animator.StringToHash("Defending");
			jumpingBool = Animator.StringToHash("Jumping");
			
			speedFloat = Animator.StringToHash("Speed");
			
			MotionLockedHash = Animator.StringToHash("MotionLocked");	
		//}
	}
	
	void FixedUpdate()
	{
		//if (networkView.isMine) 
		//{
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			bool run = Input.GetButton("Run");
			
			// Do not process movement and rotation if you are in motionlocked.
			//if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("MotionLocked"))
				MovementManagement(h, v, run);
		//}

	}
	
	void MovementManagement(float horizontal, float vertical, bool running)
	{
		characterSpeed = animator.GetFloat(speedFloat);
		Debug.Log ("Horizontal: " + horizontal);
		Debug.Log ("Vertical: " + vertical);
		if (horizontal != 0f || vertical != 0f)
		{
			animator.SetBool(runningBool, running);
			Vector3 newX = Camera.main.transform.right * horizontal;
			Vector3 newY = Camera.main.transform.forward * vertical;
			newX = new Vector3(newX.x,0f,newX.z);
			newY = new Vector3(newY.x, 0f, newY.z);
			inputVector = newX + newY;
			Rotating(horizontal, vertical, inputVector);
			if (running)
				animator.SetFloat(speedFloat, Mathf.Lerp(characterSpeed, topRunSpeed, Time.deltaTime*runDampTime));
			else
				animator.SetFloat(speedFloat, Mathf.Lerp(characterSpeed, topWalkSpeed, Time.deltaTime*walkDampTime));
		}
		else
		{
			// do a gradual deceleration (the same time frame as immediate acceleration)
			animator.SetFloat(speedFloat, Mathf.Lerp(characterSpeed, 0f, Time.deltaTime*runDampTime));
			animator.SetBool (runningBool, false);
		}
		if (characterSpeed < 0.1)
		{
			//characterSpeed = 0;
			//controller.Move(new Vector3(0f, ApplyGravity(), 0f));
		}
		else
			controller.Move(inputVector * characterSpeed * Time.deltaTime);
	}

	float ApplyGravity()
	{
		if (fallingSpeed < terminalVelocity)
		{
			fallingSpeed += gravity*Time.deltaTime;
		}
		//else if (terminalVelocity <= fallingSpeed)
		//{
		//	fallingSpeed = terminalVelocity;
		//}
		if (controller.isGrounded)
		{
			fallingSpeed = 0f;
		}
		return -fallingSpeed;
	}
	
	void Rotating(float horizontal, float vertical, Vector3 direction)
	{
		Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		transform.rotation = newRotation;
	}
}