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

		if (Input.GetButton("Test"))
		{
			animator.SetTrigger(hash.hurtTrigger);
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

	#region Animation Events

	public void OnEventAttack()
	{
		animator.SetBool(hash.attackingBool, false);
	}

	public void OnEventSmashOff()
	{
		animator.SetBool(hash.smashingBool, false);
	}

	/* This function will snap the player's rotation to the rotation of 
	 * the camera on the y axis. The main use for this is when attacking.
	 */

	public void OnEventSnapToCamera()
	{
		Debug.Log("Snapped Player to Camera y axis");
		transform.rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
	}

	#endregion
}
