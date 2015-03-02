using UnityEngine;
using System.Collections;

public class HitCollision : Photon.MonoBehaviour {
	private PlayerAnimator playeranimator;
	private Animator animator;
	private HashIDs hash;
	private PlayerHealth playerHealth;



	void Start() 
	{
		if (photonView.isMine)
		{
			playeranimator = GetComponent<PlayerAnimator>();
			playerHealth = GetComponent<PlayerHealth>();

		}
	}

	// Update is called once per frame
	void Update () {
		if (photonView.isMine)
		{
			if (playerHealth.currentHealth <= 0)
			{
				playeranimator.Die();
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("I have collided!");
		Debug.Log ("photonView: "+photonView);
		if (photonView.isMine)
		{
			Debug.Log ("this: " + this);
			Debug.Log ("NetworkPlayer : "+Network.player.ToString());
			//Debug.Log (other.collider.transform.parent.gameObject);
			//Debug.Log ("OtherNetworkPlayer : "+other.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject.networkView.owner);
			//string player = other.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject.networkView.owner.ToString();
			//if(!player.Equals(Network.player.ToString()))
			//{
			playeranimator.Hurt();
			playerHealth.TakeDamage(15);
			Debug.Log ("afterTAkeDamage");
			//}

		}
	}

	public void OnEventAttackBegin()
	{
		if (!photonView.isMine)
		{
			GameObject.FindGameObjectWithTag("Weapon").GetComponent<BoxCollider>().enabled = true;
			Debug.Log ("Attack enabled");
		}
	}

	public void OnEventAttackEnd()
	{
		if (!photonView.isMine)
		{
			GameObject.FindGameObjectWithTag("Weapon").GetComponent<BoxCollider>().enabled = false;
			Debug.Log ("Attack disabled");
		}
	}

	public void HealthTest()
	{
		Debug.Log ("why are you hitting yourself?");
		playerHealth.TakeDamage(15);
	}
}
