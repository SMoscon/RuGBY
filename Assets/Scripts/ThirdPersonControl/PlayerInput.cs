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

	private int dodgingTrigger;
	
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
		dodgingTrigger = Animator.StringToHash("Dodging");
		
		speedFloat = Animator.StringToHash("Speed");
		
		ActionLockedHash = Animator.StringToHash("ActionLocked");
		DefendingHash = Animator.StringToHash("Defending");
		AttackingHash = Animator.StringToHash("Attacking");
	}

	void Update()
	{
		currentTagHash = animator.GetCurrentAnimatorStateInfo(0).tagHash;
		CheckPlayerInput(currentTagHash);
		CheckEndConditions(currentTagHash);
	}

	void CheckPlayerInput(int taghash)
	{
		// if we are locked in an animation that cannot be interrupted, don't check for input
		if (taghash == ActionLockedHash)
			return;

		if (Input.GetButtonDown("Attack"))
		{
			animator.SetBool(attackingBool, true);
		}

		// if you are attacking then the smash button will smash, else it will defend
		if (Input.GetButton("Smash") && currentTagHash == AttackingHash)
		{
			animator.SetBool(smashingBool, true);
		}
		else if (Input.GetButton("Smash"))
		{
			animator.SetBool(defendingBool, true);
		}
		
		if (Input.GetButtonDown("Dodge"))
		{
			animator.SetTrigger(dodgingTrigger);
		}
	}

	void CheckEndConditions(int taghash)
	{
		// if we are locked in an animation that cannot be interrupted, wait for it to finish
		if (taghash == ActionLockedHash)
		{
			return;
		}

		if (currentTagHash == DefendingHash)
		{
			animator.SetBool(defendingBool, Input.GetButton("Smash"));
		}
	}

	#region Animation Events to deal with Animator States

	public void OnEventAttack()
	{
		animator.SetBool(attackingBool, false);
	}

	public void OnEventSmashOff()
	{
		animator.SetBool(smashingBool, false);
	}

	#endregion
}
