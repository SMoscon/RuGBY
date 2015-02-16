/* Takes the MoveVector and processes it into a game world
 * motion with respect / relative to the camera direction
 * 
 * i.e. if you are facing south and you press forward you go south.
 */
using UnityEngine;
using System.Collections;

public class TP_Motor : MonoBehaviour
{
	public static TP_Motor Instance;
	
	public float ForwardSpeed = 10f;
	public float RunSpeed = 15f;
	public float BackwardSpeed = 2f;
	public float JumpSpeed = 12f;
	public float Gravity = 10f;
	public float TerminalVelocity = 20f;
	public float SlideThreshold = 0.6f;
	public float MaxControllableSlideMagnitude = 0.4f;
	public float RotationSpeed = 0.2f;
	public float StartingRotation;
	public float EndingRotation;
	
	//private Vector3 slideDirection;
	
	public Vector3 MoveVector { get; set; }
	public float VerticalVelocity {get; set; }
	//public bool IsSliding { get; set; }

	/*
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = transform.position;
			stream.Serialize(ref syncPosition);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			transform.position = syncPosition;
		}
	}*/


	void Awake() 
	{
		if (networkView.isMine) 
		{
			Instance = this;
		}
	}
	
	public void UpdateMotor() 
	{
		ProcessMotion();
		UpdateRotation();
	}
	
	void ProcessMotion()
	{
		// Transform MoveVector to World Space
		MoveVector = transform.forward;
		MoveVector = new Vector3 (MoveVector.x, 0, MoveVector.z);
		//Debug.Log (MoveVector.ToString ());
		// Normalize MoveVector if Magnitude > 1
		if (MoveVector.magnitude > 1)
			MoveVector = Vector3.Normalize(MoveVector);
		
		// Apply sliding if applicable
		//ApplySlide();
		
		
		// Multiply MoveVector by MoveSpeed
		MoveVector *= MoveSpeed();
		
		// reapply verticalvelocity to movevector.y
		MoveVector = new Vector3(MoveVector.x, VerticalVelocity , MoveVector.z);
		
		//Apply gravity
		ApplyGravity();

		
		// Move the Character in World Space
		if (TP_Animator.Instance.MoveDirection == TP_Animator.Direction.Locked) 
		{
			MoveVector = new Vector3(0, MoveVector.y, 0);
		}
		else
			TP_Controller.CharacterController.Move(MoveVector * Time.deltaTime);
	}
	
	void ApplyGravity()
	{
		if (MoveVector.y > -TerminalVelocity)
		{
			MoveVector = new Vector3(MoveVector.x, MoveVector.y - Gravity * Time.deltaTime , MoveVector.z);	
		}
		
		if (TP_Controller.CharacterController.isGrounded && MoveVector.y < -1)
		{
			MoveVector = new Vector3(MoveVector.x, -1 , MoveVector.z);		
		}
		
	}
	
	//void ApplySlide()
	//{
	//	if (!TP_Controller.CharacterController.isGrounded)
	//		return;
	//	
	//	slideDirection = Vector3.zero;
	//	
	//	RaycastHit hitInfo;
	//	
	//	if (Physics.Raycast(transform.position, Vector3.down, out hitInfo))
	//	{
	//		// If the "terrain" is steep enough, create a slide direction from the normal
	//		if (hitInfo.normal.y < SlideThreshold)
	//		{
	//			slideDirection = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);
	//			if (!IsSliding)
	//			{
	//				TP_Animator.Instance.Slide();
	//			}
	//			IsSliding = true;
	//		}
	//		else
	//			IsSliding = false;
	//	}
	//	
	//	// Check if the "terrain" is too step to be able to control the slide
	//	if (slideDirection.magnitude < MaxControllableSlideMagnitude)
	//	{
	//		MoveVector += slideDirection;
	//	}
	//	else
	//	{
	//		MoveVector = slideDirection;
	//	}
	//}
	
	public void Jump()
	{
		if (TP_Controller.CharacterController.isGrounded && TP_Animator.Instance.State != TP_Animator.CharacterState.Landing)
		{
			VerticalVelocity = JumpSpeed;
		}
	}

	public void AttackRotation()
	{
		//Debug.Log (transform.eulerAngles.y == Camera.main.transform.eulerAngles.y);
		if (!TP_Animator.Instance.TurnAttack)
			return;
		if (transform.eulerAngles.y == EndingRotation) 
		{
			TP_Animator.Instance.TurnAttack = false;
			return;
		}
		transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 
		                                      Mathf.Lerp(EndingRotation, StartingRotation, (RotationSpeed - Time.deltaTime) / RotationSpeed),
		                                      transform.eulerAngles.z);
	}
	
	public void UpdateRotation()
	{
		if (TP_Animator.Instance.MoveDirection == TP_Animator.Direction.Locked)
			return;
		switch (TP_Animator.Instance.MoveDirection) 
		{
			case TP_Animator.Direction.Forward:
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
				break;
			case TP_Animator.Direction.Backward:
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y - 180, transform.eulerAngles.z);
				break;
			case TP_Animator.Direction.Left:
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y - 90, transform.eulerAngles.z);
				break;
			case TP_Animator.Direction.Right:
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y + 90, transform.eulerAngles.z);
				break;
			case TP_Animator.Direction.LeftForward:
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y - 45, transform.eulerAngles.z);
				break;
			case TP_Animator.Direction.RightForward:
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y + 45, transform.eulerAngles.z);
				break;
			case TP_Animator.Direction.LeftBackward:
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y - 135, transform.eulerAngles.z);
				break;
			case TP_Animator.Direction.RightBackward:
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y + 135, transform.eulerAngles.z);
				break;
		}

		//if ((MoveVector.x != 0 || MoveVector.z != 0 || TP_Animator.Instance.State == TP_Animator.CharacterState.Attacking) && TP_Animator.Instance.MoveDirection != TP_Animator.Direction.Backward) 
		//{
		//	transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
		//	                                      Camera.main.transform.eulerAngles.y,
		//	                                      transform.eulerAngles.z);
		//}
	}

	float MoveSpeed()
	{
		if (TP_Animator.Instance.State == TP_Animator.CharacterState.Running)
			return RunSpeed;
		else if (TP_Animator.Instance.MoveDirection == TP_Animator.Direction.Stationary || TP_Animator.Instance.MoveDirection == TP_Animator.Direction.Locked)
			return 0f;
		//else if (IsSliding)
		//	return SlideSpeed;
		else
			return ForwardSpeed;
	}
}
