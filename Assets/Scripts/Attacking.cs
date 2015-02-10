using UnityEngine;
using System.Collections;

public class Attacking : MonoBehaviour
{
	//public Rigidbody arrowHitBox;
	public Rigidbody spearHitBox;
	//public Transform bowPosition;
	//public float projectileSpeed;
	
	//private Inventory inventory;
	//private PlayerController playerController;
	
	//void Awake()
	//{
		//inventory = GetComponent<Inventory>();
		//playerController = GetComponent<PlayerController>();

	//}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Attack();
		}
	}
	
	void Attack()
	{
		Debug.Log ("Attack occured");
		//transform.rotation = Vector3.RotateTowards (Camera.main.target.position);
		//Animation.Play ("Yellow_Rig|Yellow_Jump");
	}
}
