﻿using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
	public int idleState;
	public int dyingState;
	public int locomotionState;
	public int jumpingState;
	public int attackingState;

	public int attackingBool;
	public int smashingBool;
	public int runningBool;
	public int defendingBool;
	public int jumpingBool;

	public int speedFloat;

	public int MotionLockedHash;
	
	void Awake ()
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

		speedFloat = Animator.StringToHash("Speed");

		MotionLockedHash = Animator.StringToHash("MotionLocked");

	}
}