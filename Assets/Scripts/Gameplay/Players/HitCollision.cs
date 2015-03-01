using UnityEngine;
using System.Collections;

public class HitCollision : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (photonView.isMine){
			if (HealthBar.Instance.curHealth==0){
				//Die here
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("I have collided!");
		Debug.Log ("photonView: "+photonView);
		if (this.photonView.isMine)
		{/*
			Debug.Log ("this: " + this);
			Debug.Log ("NetworkPlayer : "+Network.player.ToString());
			Debug.Log (other.collider.transform.parent.gameObject);
			Debug.Log ("OtherNetworkPlayer : "+other.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject.networkView.owner);
			string player = other.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject.networkView.owner.ToString();
			if(!player.Equals(Network.player.ToString())){
				HealthBar.Instance.AdjustCurrentHealth(-15);
			}*/

		}
	}
}
