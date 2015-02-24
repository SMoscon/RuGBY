using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour 
{
	private Animator animator;
	private HashIDs hash;

	private int currentTagHash;
	
	void Start()
	{
		animator = GetComponent<Animator>();
		hash = GetComponent<HashIDs>();
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
		if (taghash == hash.ActionLockedTagHash)
			return;

		if (Input.GetButtonDown("Attack"))
		{
			animator.SetBool(hash.attackingBool, true);
		}

		// if you are attacking then the smash button will smash, else it will defend
		if (Input.GetButton("Smash") && currentTagHash == hash.AttackingTagHash)
		{
			animator.SetBool(hash.smashingBool, true);
		}
		else if (Input.GetButton("Smash"))
		{
			animator.SetBool(hash.defendingBool, true);
		}
		
		if (Input.GetButtonDown("Dodge"))
		{
			animator.SetTrigger(hash.dodgingTrigger);
		}
	}

	void CheckEndConditions(int taghash)
	{
		// if we are locked in an animation that cannot be interrupted, wait for it to finish
		if (taghash == hash.ActionLockedTagHash)
		{
			return;
		}

		if (currentTagHash == hash.DefendingTagHash)
		{
			animator.SetBool(hash.defendingBool, Input.GetButton("Smash"));
		}
	}

	#region Animation Events to deal with Animator States

	public void OnEventAttack()
	{
		animator.SetBool(hash.attackingBool, false);
	}

	public void OnEventSmashOff()
	{
		animator.SetBool(hash.smashingBool, false);
	}

	#endregion
}
