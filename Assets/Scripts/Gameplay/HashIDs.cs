using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
	[HideInInspector] public int idleState;
	[HideInInspector] public int dyingState;
	[HideInInspector] public int locomotionState;
	[HideInInspector] public int jumpingState;
	[HideInInspector] public int attackingState;
	
	[HideInInspector] public int attackingBool;
	[HideInInspector] public int smashingBool;
	[HideInInspector] public int runningBool;
	[HideInInspector] public int defendingBool;
	[HideInInspector] public int jumpingBool;
	[HideInInspector] public int deadBool;
	
	[HideInInspector] public int dodgingTrigger;
	[HideInInspector] public int hurtTrigger;
	
	[HideInInspector] public int speedFloat;
	
	[HideInInspector] public int ActionLockedTagHash;
	[HideInInspector] public int DefendingTagHash;
	[HideInInspector] public int AttackingTagHash;
	[HideInInspector] public int DodgingTagHash;

	void Awake()
	{
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
		deadBool = Animator.StringToHash("Dead");

		dodgingTrigger = Animator.StringToHash("Dodging");
		hurtTrigger = Animator.StringToHash("Hurt");
		
		speedFloat = Animator.StringToHash("Speed");
		
		ActionLockedTagHash = Animator.StringToHash("ActionLocked");
		DefendingTagHash = Animator.StringToHash("Defending");
		AttackingTagHash = Animator.StringToHash("Attacking");
		DodgingTagHash = Animator.StringToHash("Dodging");

	}
}