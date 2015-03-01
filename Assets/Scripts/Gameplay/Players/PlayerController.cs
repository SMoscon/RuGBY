using UnityEngine;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour
{
	public float turnSmoothing = 15f;
	public float walkDampTime = 5f;
	public float runDampTime = 10f;
	public float characterSpeed;
	public float topRunSpeed = 6f;
	public float topWalkSpeed = 3f;
	public float fallingSpeed;
	public float gravity = 0.2f;
	public float rotSpeed = 3f;

	public float horizontal;
	public float vertical;

	public Vector3 inputVector = Vector3.zero;

	private int currentTagHash;

	private Animator animator;
	private CharacterController controller;
	private HashIDs hash;

	void Awake()
	{
		animator = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
		hash = GetComponent<HashIDs>();
	}
	
	void FixedUpdate()
	{
		if(photonView.isMine)
		{
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");
			bool run = Input.GetButton("Run");
			
			// Do not process movement and rotation if you are in motionlocked.
			currentTagHash = animator.GetCurrentAnimatorStateInfo(0).tagHash;
			if (currentTagHash == hash.ActionLockedTagHash || currentTagHash == hash.DefendingTagHash || currentTagHash == hash.AttackingTagHash
			    || currentTagHash == hash.DodgingTagHash)
			{
				controller.Move(new Vector3(0f, ApplyGravity(), 0f)*Time.deltaTime);
			}
			else
			{
				MovementManagement(horizontal, vertical, run, inputVector);
			}
		}
	}

	void MovementManagement(float h, float v, bool running, Vector3 inputVector)
	{
		characterSpeed = animator.GetFloat(hash.speedFloat);
		if (horizontal != 0f || vertical != 0f)
		{
			Vector3 newX = Camera.main.transform.right * h;
			Vector3 newY = Camera.main.transform.forward * v;
			inputVector = newX + newY;
			inputVector = new Vector3(inputVector.x, 0f, inputVector.z);
			Rotating(inputVector);
			if (running)
			{
				animator.SetFloat (hash.speedFloat, Mathf.Lerp(characterSpeed, topRunSpeed, Time.deltaTime*runDampTime));
				//photonView.RPC("SetFloat", PhotonTargets.All, hash.speedFloat, Mathf.Lerp(characterSpeed, topRunSpeed, Time.deltaTime*runDampTime));
			}
			else
			{
				animator.SetFloat (hash.speedFloat, Mathf.Lerp(characterSpeed, topWalkSpeed, Time.deltaTime*walkDampTime));
				//photonView.RPC("SetFloat", PhotonTargets.All, hash.speedFloat, Mathf.Lerp(characterSpeed, topWalkSpeed, Time.deltaTime*walkDampTime));
			}
        }
		else
		{
			// do a gradual deceleration (the same time frame as immediate acceleration)
			animator.SetFloat (hash.speedFloat, Mathf.Lerp(characterSpeed, 0f, Time.deltaTime*runDampTime));
			//photonView.RPC("SetFloat", PhotonTargets.All, hash.speedFloat, Mathf.Lerp(characterSpeed, 0f, Time.deltaTime*runDampTime));
		}
		if (characterSpeed < 0.1)
		{
			characterSpeed = 0;
			controller.Move(new Vector3(0f, ApplyGravity(), 0f) * Time.deltaTime);
		}
		else
			controller.Move(new Vector3(inputVector.x*characterSpeed, ApplyGravity(), inputVector.z*characterSpeed)*Time.deltaTime);
	}

	float ApplyGravity()
	{
		if (!controller.isGrounded)
		{
			fallingSpeed += gravity*Time.deltaTime;
		}
		//else if (terminalVelocity <= fallingSpeed)
		//{
		//	fallingSpeed = terminalVelocity;
		//}
		else
		{
			//Debug.Log ("Grounded");
			fallingSpeed = 0f;
		}
		return -fallingSpeed;
	}
	
	void Rotating(Vector3 direction)
	{
		//if (currentTagHash != hash.DodgingTagHash)
		{
			Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
			Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
			transform.rotation = newRotation;
		}
	}

	#region Animation Events (Movement and Move Rotation only)

	public void OnEventSnapToDirection()
	{
		Vector3 newX = Camera.main.transform.right * horizontal;
		Vector3 newY = Camera.main.transform.forward * vertical;
		inputVector = newX + newY;
		inputVector = new Vector3(inputVector.x, 0f, inputVector.z);
		if (inputVector == Vector3.zero)
			inputVector = transform.forward;
		transform.rotation = Quaternion.LookRotation(inputVector, Vector3.up);
	}

	#endregion
}