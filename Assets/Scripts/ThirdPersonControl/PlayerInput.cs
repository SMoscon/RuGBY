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
	
	private int ActionLockedHash;
	private int DefendingHash;
	private int AttackingHash;
	private int currentTagHash;
	
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
		
		ActionLockedHash = Animator.StringToHash("ActionLocked");
		DefendingHash = Animator.StringToHash("Defending");
		AttackingHash = Animator.StringToHash("Attacking");
	}

	void Update()
	{
		currentTagHash = animator.GetCurrentAnimatorStateInfo(0).tagHash;

		if (Input.GetButtonDown("Attack"))
		{
			animator.SetBool(attackingBool, true);
		}

		if (currentTagHash == DefendingHash)
		{
			animator.SetBool(defendingBool, Input.GetButton("Smash"));
		}

		if (Input.GetButton("Smash") && currentTagHash == AttackingHash)
		{
			animator.SetBool(smashingBool, true);
		}
		else if (Input.GetButton("Smash"))
		{
			animator.SetBool(defendingBool, true);
		}
	}

	public void OnEventAttack()
	{
		animator.SetBool(attackingBool, false);
	}

	public void OnEventSmashOff()
	{
		animator.SetBool(smashingBool, false);
	}
}
