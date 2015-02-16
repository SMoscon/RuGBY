using UnityEngine;
using System.Collections;

public class HitCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (HealthBar.Instance.curHealth==0){
			TP_Animator.Instance.Die();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("I have collided!");
		if (!networkView.isMine)
		{
			HealthBar.Instance.AdjustCurrentHealth(-15);
		}
	}
}
