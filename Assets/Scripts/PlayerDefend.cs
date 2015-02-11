using UnityEngine;
using System.Collections;

public class PlayerDefend : MonoBehaviour {
	
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	public AnimationClip defendAnimation;
	public AnimationClip returnDefendAnimation;
	
	private int _defending;
	//public static def_shield = 4 * _defending;

	// Use this for initialization
	void Start () {
		_defending = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1)){
			Defend();
		}
		else if (Input.GetMouseButtonUp(1)){
			if (_defending == 1){
				ReleaseDef();
			}
		}
	
	}
	
	public void Defend () {
		_defending = 1;
		animation.Play(defendAnimation.name);
	}
	public void ReleaseDef() {
		_defending = 0;
		animation.Play(returnDefendAnimation.name);
	}
}