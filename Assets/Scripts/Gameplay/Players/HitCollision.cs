using UnityEngine;
using System.Collections;

public class HitCollision : MonoBehaviour {

	private PlayerAnimator playeranimator;

	void Start () 
	{
		if (networkView.isMine)
		{
			playeranimator = GetComponent<PlayerAnimator>();
		}
	}

	void Update () 
	{
		if (networkView.isMine)
		{
			if (HealthBar.Instance.curHealth==0)
			{
				playeranimator.Die();
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("I have collided!");
		Debug.Log ("networkView: "+networkView);
		if (this.networkView.isMine)
		{
			Debug.Log ("this: " + this);
			Debug.Log ("NetworkPlayer : "+Network.player.ToString());
			Debug.Log (other.collider.transform.parent.gameObject);
			Debug.Log ("OtherNetworkPlayer : "+other.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject.networkView.owner);
			string player = other.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject.networkView.owner.ToString();
			if(!player.Equals(Network.player.ToString()))
			{
				HealthBar.Instance.AdjustCurrentHealth(-15);
				playeranimator.Hurt();
			}

		}
	}
}
