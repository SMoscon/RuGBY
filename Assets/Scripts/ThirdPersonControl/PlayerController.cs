using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	private int speedFloat;
	private int attackBool;
	private int smashBool;
	private int jumpBool;
	private int defendBool;
	private int runningBool;
	private int MotionLockedTag;

	public float turnSmoothing = 15f;
	public float walkDampTime = 5f;
	public float runDampTime = 10f;
	public float characterSpeed;
	public float topRunSpeed = 6f;
	public float topWalkSpeed = 3f;

	private Animator animator;
	private CharacterController controller;
	
	void Awake()
	{
		animator = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
		speedFloat = Animator.StringToHash("Speed");
		attackBool = Animator.StringToHash("Attacking");
		smashBool = Animator.StringToHash("Smashing");
		jumpBool = Animator.StringToHash("Jumping");
		defendBool = Animator.StringToHash("Defending");
		runningBool = Animator.StringToHash("Running");
	}
	
	
	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		bool run = Input.GetButton("Run");

		// Do not process movement and rotation if you are in motionlocked.
		if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("MotionLocked"))
			MovementManagement(h, v, run);
	}
	
	
	void MovementManagement(float horizontal, float vertical, bool running)
	{
		characterSpeed = animator.GetFloat(speedFloat);
		if(horizontal != 0f || vertical != 0f)
		{
			animator.SetBool(runningBool, running);
			Rotating(horizontal, vertical);
			if (running)
				animator.SetFloat(speedFloat, Mathf.Lerp(characterSpeed, topRunSpeed, Time.deltaTime*runDampTime));
			else
				animator.SetFloat(speedFloat, Mathf.Lerp(characterSpeed, topWalkSpeed, Time.deltaTime*walkDampTime));
			//controller.Move(transform.forward.normalized * animator.GetFloat(speedFloat) * Time.deltaTime);
		}
		else
		{
			animator.SetFloat(speedFloat, Mathf.Lerp(characterSpeed, 0f, Time.deltaTime*runDampTime));
			animator.SetBool (runningBool, false);
		}
		controller.Move(transform.forward.normalized*characterSpeed*Time.deltaTime);
	}
	
	
	void Rotating(float horizontal, float vertical)
	{
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);

		transform.rotation = newRotation;
	}
}