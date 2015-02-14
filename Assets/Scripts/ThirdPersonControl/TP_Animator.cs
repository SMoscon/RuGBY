using UnityEngine;
using System.Collections;

public class TP_Animator : MonoBehaviour 
{
	public AnimationClip idleAnimation;
	public AnimationClip walkAnimation;
	public AnimationClip runAnimation;
	public AnimationClip usingAnimation;
	public AnimationClip jumpAnimation;
	public AnimationClip hardlandAnimation;
	public AnimationClip runlandingAnimation;
	public AnimationClip landingAnimation;
	public AnimationClip fallingAnimation;
	//public AnimationClip climbingAnimation;

	public AnimationClip defendAnimation;
	public AnimationClip defendreturnAnimation;
	public AnimationClip firstattackAnimation;
	public AnimationClip secondattackAnimation;
	public AnimationClip thirdattackAnimation;
	public AnimationClip firstsmashAnimation;
	public AnimationClip secondsmashAnimation;
	public AnimationClip thirdsmashAnimation;
	public AnimationClip firstsmashreturnAnimation;
	public AnimationClip secondsmashreturnAnimation;
	public AnimationClip thirdsmashreturnAnimation;
	public AnimationClip firstattackreturnAnimation;
	public AnimationClip secondattackreturnAnimation;
	public AnimationClip thirdattackreturnAnimation;

	public enum Direction
	{
		Stationary, Forward, Backward, Left, Right, 
		LeftForward, RightForward, LeftBackward, RightBackward,
		RunForward
	}

	public enum CharacterState
	{
		Idle, Walking, Running, Dodging, Falling, Landing, Jumping, Attacking, 
		Using, Defending, Dead, Climbing, Sliding
	}
	
	public static TP_Animator Instance;

	private CharacterState lastState;
	//private Transform climbPoint;

	//public Vector3 ClimbOffset = Vector3.zero;
	//public Vector3 PostClimbOffset = Vector3.zero;
	// when the character actually starts jumping
	//public float ClimbJumpStartTime = 0f;
	//public float ClimbAnchorTime = 0.6f; // in seconds

	//private Transform abdomen;

	private Vector3 initialPosition = Vector3.zero;
	private Quaternion initialRotation = Quaternion.identity;


	public Direction PreviousDirection { get; set; }
	public Direction MoveDirection { get; set; }
	public CharacterState State { get; set; }
	public bool IsDead { get; set; }

	public int ComboCounter { get; set; }
	public bool IsSmashing { get; set; }
	public bool IsAttacking { get; set; }
	public bool EndAttack { get; set; }
	public bool IsDefending { get; set; }
	

	void Awake() 
	{
		if (networkView.isMine) {
			Instance = this;
			initialPosition = transform.position;
			initialRotation = transform.rotation;
			IsDefending = false;
			IsAttacking = false;
			IsSmashing = false;
			ComboCounter = 0;
			EndAttack = false;
		}
	}

	void Update()
	{
		DetermineCurrentState();
		ProcessCurrentState();
		//Debug.Log("Current State: " + State.ToString());
	}

	
	public void DetermineCurrentMoveDirection()
	{
		var forward = false;
		var backward = false;
		var right = false;
		var left = false;

		PreviousDirection = MoveDirection;
		
		if (TP_Motor.Instance.MoveVector.z > 0) 
		{
			forward = true;
		}
		if (TP_Motor.Instance.MoveVector.z < 0) 
		{
			backward = true;
		}
		if (TP_Motor.Instance.MoveVector.x > 0) 
		{
			right = true;
		}
		if (TP_Motor.Instance.MoveVector.x < 0) 
		{
			left = true;
		}
		
		if (forward)
		{
			if (left)
			{
				MoveDirection = Direction.LeftForward;
			}
			else if (right)
			{
				MoveDirection = Direction.RightForward;	
			}
			else 
			{
				MoveDirection = Direction.Forward;
			}
		}
		else if (backward)
		{
			if (left)
			{
				MoveDirection = Direction.LeftBackward;
			}
			else if (right)
			{
				MoveDirection = Direction.RightBackward;	
			}
			else
			{
				MoveDirection = Direction.Backward;
			}
		}
		else if (left)
		{
			MoveDirection = Direction.Left;
		}
		else if (right)
		{
			MoveDirection = Direction.Right;
		}
		else
		{
			MoveDirection = Direction.Stationary;	
		}

		if (MoveDirection != PreviousDirection)
			TP_Motor.Instance.UpdateRotation();
	}

	//what is our state currently?
	void DetermineCurrentState()
	{
		if (State == CharacterState.Dead)
			return;
		if (!TP_Controller.CharacterController.isGrounded) 
		{
			if  (State != CharacterState.Falling && 
			    State != CharacterState.Jumping &&
			    State != CharacterState.Landing)
			{
				Fall();
			}
		}
		if 	(State != CharacterState.Falling && 
			State != CharacterState.Landing &&
			State != CharacterState.Jumping &&
			State != CharacterState.Using &&
		    State != CharacterState.Running &&
			State != CharacterState.Climbing &&
			State != CharacterState.Attacking &&
			State != CharacterState.Defending &&
			State != CharacterState.Sliding) 
		{
			switch (MoveDirection)
			{
				case Direction.Stationary:
					State = CharacterState.Idle;
					break;
				case Direction.Forward:
					State = CharacterState.Walking;
					break;
				case Direction.Backward:
					State = CharacterState.Walking;
					break;
				case Direction.Left:
					State = CharacterState.Walking;
					break;
				case Direction.Right:
					State = CharacterState.Walking;
					break;
				case Direction.LeftForward:
					State = CharacterState.Walking;
					break;
				case Direction.RightForward:
					State = CharacterState.Walking;
					break;
				case Direction.LeftBackward:
					State = CharacterState.Walking;
					break;
				case Direction.RightBackward:
					State = CharacterState.Walking;
					break;
			}
		}

	}

	//determine state and act on it
	void ProcessCurrentState()
	{
		switch (State) 
		{
			case CharacterState.Idle:
				Idle();
				break;
			case CharacterState.Walking:
				Walking();
				break;
			case CharacterState.Using:
				Using();
				break;
			case CharacterState.Jumping:
				Jumping();
				break;
			case CharacterState.Falling:
				Falling();
				break;
			case CharacterState.Landing:
				Landing();
				break;
			//case CharacterState.Sliding:
			//	Sliding();
			//	break;
			//case CharacterState.Climbing:
			//	Climbing();
			//	break;
			case CharacterState.Dead:
				break;
			case CharacterState.Attacking:
				Attacking();
				break;
			case CharacterState.Defending:
				Defending();
				break;
		}
	}

	// Character State Methods below (called every update)

	void Idle()
	{
		animation.CrossFade(idleAnimation.name);
	}

	void Walking()
	{
		animation.CrossFade(walkAnimation.name);
	}

	void Running()
	{
		animation.CrossFade(runAnimation.name);
	}

	void Using()
	{
		if (!animation.isPlaying)
		{
			State = CharacterState.Idle;
			animation.CrossFade(idleAnimation.name);
		}
	}

	void Jumping()
	{
		if ((!animation.isPlaying && TP_Controller.CharacterController.isGrounded) ||
			TP_Controller.CharacterController.isGrounded)
		{
			if (lastState == CharacterState.Running || lastState == CharacterState.Walking)
				animation.CrossFade(runlandingAnimation.name);
			else
				animation.CrossFade(landingAnimation.name);
			State = CharacterState.Landing;
		}
		else if (!animation.IsPlaying(jumpAnimation.name))
		{
			Fall();
		}
		else
		{
			State = CharacterState.Jumping;
			//Help determine if we fell too far??
		}
	}

	void Falling()
	{
		if (TP_Controller.CharacterController.isGrounded)
		{
			if (lastState == CharacterState.Running || lastState == CharacterState.Walking)
				animation.CrossFade(runlandingAnimation.name);
			else if (lastState == CharacterState.Falling)
				animation.CrossFade(hardlandAnimation.name);
			else
				animation.CrossFade(landingAnimation.name);
			State = CharacterState.Landing;
		}
	}

	void Landing()
	{
		if (lastState == CharacterState.Running)
		{
			if (!animation.IsPlaying(runlandingAnimation.name))
			{
				State = CharacterState.Running;
				animation.CrossFade(runAnimation.name);
			}	
		}
		else
		{
			if (!animation.IsPlaying(landingAnimation.name))
			{
				State = CharacterState.Idle;
				animation.CrossFade(idleAnimation.name);
			}
		}
	}

	//void Sliding()
	//{
	//	if (!TP_Motor.Instance.IsSliding)
	//	{
	//		State = CharacterState.Idle;
	//		animation.CrossFade(idleAnimation.name);
	//	}
	//}

	// is an animation playing? if not determine what animation should be playing or if we should end
	void Attacking()
	{
		if (!animation.isPlaying && IsAttacking && !EndAttack)
		{
			if (IsSmashing)
			{
				IsAttacking = false;
				switch (ComboCounter)
				{
					case 1:
						animation.Play(firstsmashAnimation.name);
				  		break;
					case 2:
						animation.Play(secondsmashAnimation.name);
						break;
					case 3:
						animation.Play(thirdsmashAnimation.name);
						break;
				}
			}
			else
			{
				IsAttacking = false;
				ComboCounter++;
				switch (ComboCounter)
				{
					case 1:
						animation.Play(firstattackAnimation.name);
						break;
					case 2:
						animation.Play(secondattackAnimation.name);
						break;
					case 3:
						animation.Play(thirdattackAnimation.name);
						break;
				}
			}
		}
		else if (!animation.isPlaying && !IsAttacking && !EndAttack)
		{
			if (IsSmashing)
			{	
				switch (ComboCounter)
				{
					case 1:
						animation.Play(firstsmashreturnAnimation.name);
						break;
					case 2:
						animation.Play(secondsmashreturnAnimation.name);
						break;
					case 3:
						animation.Play(thirdsmashreturnAnimation.name);
						break;
				}
				EndAttack = true;
			}
			else
			{
				switch (ComboCounter)
				{
					case 1:
						animation.Play(firstattackreturnAnimation.name);				
						break;
					case 2:
						animation.Play(secondattackreturnAnimation.name);
						break;
					case 3:
						animation.Play(thirdattackreturnAnimation.name);
						break;
				}
				EndAttack = true;
			}
		}
		else if (!animation.isPlaying && EndAttack)
		{
			EndAttack = false;
			ComboCounter = 0;
			IsSmashing = false;
			State = CharacterState.Idle;
			animation.CrossFade(idleAnimation.name);
		}
		
	}

	//void Climbing()
	//{
	//	if (animation.isPlaying)
	//	{
	//		var time = animation[climbingAnimation.name].time;
	//		if (time > ClimbJumpStartTime && time < ClimbAnchorTime)
	//		{
	//			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
	//									    Mathf.Lerp(transform.rotation.eulerAngles.y, climbPoint.rotation.eulerAngles.y, 
	//											   (time - ClimbJumpStartTime) / (ClimbAnchorTime - ClimbJumpStartTime)), 
	//									    transform.rotation.eulerAngles.z);
	//			// fine tune where we end up after jump
	//			var climbOffset = transform.TransformDirection(ClimbOffset);
	//			
	//			transform.position = Vector3.Lerp(transform.position, 
	//									new Vector3(climbPoint.position.x, transform.position.y, climbPoint.position.z) + ClimbOffset),
	//									(time - ClimbJumpStartTime) / (ClimbAnchorTime - ClimbJumpStartTime);
	//		}
	//	}
	//	else
	//	{
	//		State = CharacterState.Idle;
	//		animation.Play(idleAnimation.name);
	//		// put our charactercontroller back with the character after the animation
	//		var postClimbOffset = transform.TransformDirection(PostClimbOffset);
	//		transform.position = new Vector3(abdomen.position.x, climbPoint.position.y + climbPoint.localScale.y / 2, abdomen.position.z) + postClimbOffset;
	//	}
	//}

	void Defending()
	{
		if (IsDefending && animation.isPlaying) 
		{
			animation.CrossFade(defendAnimation.name);
		}
		else if (!IsDefending)
		{
			animation.CrossFade(defendreturnAnimation.name);
		}
		else if (!IsDefending && !animation.isPlaying)
		{

		}
	}

	//'Start Action' methods below (called once per change of state)

	public void Use()
	{
		State = CharacterState.Using;
		animation.CrossFade(usingAnimation.name);
	}

	public void Run()
	{
		State = CharacterState.Running;
		animation.CrossFade(runAnimation.name);
	}

	public void Walk()
	{
		State = CharacterState.Walking;
		MoveDirection = Direction.Forward;
		animation.CrossFade(walkAnimation.name);
	}

	public void Jump()
	{
		if (!TP_Controller.CharacterController.isGrounded || IsDead || State == CharacterState.Jumping || 
		    State == CharacterState.Attacking || State == CharacterState.Landing)
			return;

		lastState = State;
		State = CharacterState.Jumping;
		animation.CrossFade(jumpAnimation.name);
	}

	public void Fall()
	{
		if (IsDead)
			return;
	
		if (transform.position.y > 5)
			lastState = CharacterState.Falling;
		else
			lastState = State;
		State = CharacterState.Falling;
		// if we are too high start a falling state immediately
		animation.CrossFade(fallingAnimation.name);
	}

	//public void Slide()
	//{
		//State = CharacterState.Sliding;
		//animation.CrossFade(fallingAnimation.name);
	//}

	public void Attack()
	{
		if (State == CharacterState.Attacking && !EndAttack && !IsSmashing)
		{
			IsAttacking = true;
		}
		else
			State = CharacterState.Attacking;
	}

	public void SmashAttack()
	{
		IsAttacking = true;
		IsSmashing = true;
	}

	public void Defend()
	{
		State = CharacterState.Defending;
		animation.CrossFade(defendAnimation.name);
		IsDefending = true;
	}

	//public void Climb()
	//{
	//	if (!TP_Controller.CharacterController.IsGrounded || IsDead || climbPoint == null)
	//		return;
	//	// Check your rotation in respect to the climbingvolume, if not proper rotation just jump
	//	if (Mathf.Abs(climbPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y) > 60)
	//	{
	//		TP_Controller.Instance.Jump();
	//	}
	//
	//	State = CharacterState.Climbing;
	//	animation.CrossFade(climbingAnimation.name);
	//}

	//public void SetClimbPoint(Transform climbPoint)
	//{
	//	this.climbPoint = climbPoint;
	//	TP_Controller.Instance.ClimbEnabled = true;
	//}

	//public void ClearClimbPoint(Transform climbPoint)
	//{
	//	if (this.climbPoint == climbPoint)
	//	{
	//		this.climbPoint = null;
	//		TP_Controller.Instance.ClimbEnabled = false;
	//	}
	//}

	public void Reset()
	{
		transform.position = initialPosition;
		transform.rotation = initialRotation;
	}
}