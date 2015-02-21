using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour 
{
	private Animator animator;
	private CharacterController controller;

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
	
	void Start()
	{
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
	}

	void Update()
	{
		if (Input.GetButton("Attack"))
		{
			animator.SetBool(attackingBool, true);
		}

		if (Input.GetButton("Smash"))
		{
			animator.SetBool(smashingBool, true);
		}
	}

	public void SetAttackFalse()
	{
		animator.SetBool(attackingBool, false);
	}
}
