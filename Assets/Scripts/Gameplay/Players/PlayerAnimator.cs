using UnityEngine;
using System.Collections;

public class PlayerAnimator : Photon.MonoBehaviour 
{
	private Animator animator;
	private HashIDs hash;
	private HitCollision hitcollision;
	private GameManager manager;
	private Stocks stocks;
	private PlayerController controller;
	private PlayerHealth health;

	private int currentTagHash;
	
	void Start()
	{
		health = GetComponent<PlayerHealth>();
		controller = GetComponent<PlayerController>();
		animator = GetComponent<Animator>();
		hash = GetComponent<HashIDs>();
		hitcollision = GetComponent<HitCollision>();
		manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		stocks = GetComponent<Stocks>();
	}

	void Update()
	{
        if (photonView.isMine)
        {
			currentTagHash = animator.GetCurrentAnimatorStateInfo(0).tagHash;
			CheckPlayerInput(currentTagHash);
			CheckEndConditions(currentTagHash);
		}
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
		if (Input.GetButton("Smash") && taghash == hash.AttackingTagHash)
		{
			animator.SetBool(hash.smashingBool, true);
		}
		else if (Input.GetButton("Smash"))
		{
			animator.SetBool(hash.defendingBool, true);
		}

		if (Input.GetButton("Test") && taghash != hash.DefendingTagHash)
		{
			hitcollision.HealthTest();
		}

		if (Input.GetButton("Jump") && taghash != hash.JumpingTagHash)
		{
			PhotonNetwork.RPC(photonView, "SetAnimationTrigger", PhotonTargets.Others, false, hash.jumpingTrigger);
			animator.SetTrigger(hash.jumpingTrigger);
		}

		if (Input.GetButtonDown("Dodge") && taghash != hash.DodgingTagHash)
		{
			PhotonNetwork.RPC(photonView, "SetAnimationTrigger", PhotonTargets.Others, false, hash.dodgingTrigger);
			animator.SetTrigger(hash.dodgingTrigger);
		}
	}

	void CheckEndConditions(int taghash)
	{
		// if we are locked in an animation that cannot be interrupted, wait for it to finish
		//if (taghash == hash.ActionLockedTagHash)
		//{
		//	return;
		//}

		if (currentTagHash == hash.DefendingTagHash)
		{
			animator.SetBool(hash.defendingBool, Input.GetButton("Smash"));
		}

	}

	[RPC]
	void SetAnimationTrigger (int triggerHash)
	{
		animator.SetTrigger(triggerHash);
	}

	#region public animation changes

	public void Die()
	{
		animator.SetBool(hash.deadBool, true);
	}

	public void Hurt()
	{
		animator.SetTrigger(hash.hurtTrigger);
	}

	#endregion

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

	public void OnEventDie()
	{
		stocks.AdjustStocks(-1);
		Debug.Log (stocks.GetStocks());
		if (stocks.GetStocks() <= 0)
			Debug.Log ("LOL game over noob");
		else
		{
			manager.Respawn(controller);
			health.ResetHealth();
		}
	}

	#endregion
}
