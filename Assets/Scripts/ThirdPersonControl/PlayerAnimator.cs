using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour 
{
	public static Animator animator;

	public bool Attacking {get; set;}
	public bool Defending {get; set;}
	public bool Smashing {get; set;}

	void Start() 
	{
		animator = GetComponent<Animator>();
	}
	

	void Update() 
	{

	}

	public static void Jump()
	{
		Debug.Log("Jump called");
	}
}
