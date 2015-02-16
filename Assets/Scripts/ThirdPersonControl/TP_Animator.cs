using UnityEngine;
using System.Collections;

public class TP_Animator : MonoBehaviour 
{
	// public AnimationClip idleAnimation;
	// public AnimationClip walkAnimation;
	// public AnimationClip runAnimation;
	// public AnimationClip usingAnimation;
	// public AnimationClip jumpAnimation;
	// public AnimationClip hardlandAnimation;
	// public AnimationClip runlandingAnimation;
	// public AnimationClip landingAnimation;
	// public AnimationClip fallingAnimation;
	// public AnimationClip climbingAnimation;

	// public AnimationClip defendAnimation;
	// public AnimationClip defendreturnAnimation;
	// public AnimationClip firstattackAnimation;
	// public AnimationClip secondattackAnimation;
	// public AnimationClip thirdattackAnimation;
	// public AnimationClip firstsmashAnimation;
	// public AnimationClip secondsmashAnimation;
	// public AnimationClip thirdsmashAnimation;
	// public AnimationClip firstsmashreturnAnimation;
	// public AnimationClip secondsmashreturnAnimation;
	// public AnimationClip thirdsmashreturnAnimation;
	// public AnimationClip firstattackreturnAnimation;
	// public AnimationClip secondattackreturnAnimation;
	// public AnimationClip thirdattackreturnAnimation;
	public bool AttackAnimationStarted = false;
	public int FallingBuffer = 25;
	private int FallingCounter = 0;
	public bool TurnAttack = false;

	public enum Direction
	{
		Stationary, Forward, Backward, Left, Right, 
		LeftForward, RightForward, LeftBackward, RightBackward,
		RunForward, Locked
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
		if (networkView.isMine) 
		{
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
		//Debug.Log("Direction: " + MoveDirection.ToString());
		TP_Motor.Instance.AttackRotation();
	}

	
	public void DetermineCurrentMoveDirection()
	{
		if (MoveDirection == Direction.Locked)
			return;

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
				FallingCounter++;
				if (FallingCounter > FallingBuffer)
				{
					Fall();
					FallingCounter = 0;
				}
			}
		}
		if (TP_Controller.CharacterController.isGrounded)
			FallingCounter = 0;
		if 	(State != CharacterState.Falling && 
			State != CharacterState.Landing &&
			State != CharacterState.Jumping &&
			State != CharacterState.Using &&
		    State != CharacterState.Running &&
			State != CharacterState.Climbing &&
			State != CharacterState.Attacking &&
			State != CharacterState.Defending &&
			State != CharacterState.Sliding &&
		    State != CharacterState.Dead) 
		{
			if (MoveDirection == Direction.Stationary)
			{
				State = CharacterState.Idle;
			}
			else if (MoveDirection == Direction.RunForward)
			{
				State = CharacterState.Running;
			}
			else
			{
				State = CharacterState.Walking;
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
				Dead();
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
		animation.CrossFade("Yellow_Rig|Yellow_Idle");
	}

	void Walking()
	{
		animation.CrossFade("Yellow_Rig|Yellow_Walk");
	}

	void Running()
	{
		animation.CrossFade("Yellow_Rig|Yellow_Run");
	}

	void Using()
	{
		if (!animation.isPlaying)
		{
			State = CharacterState.Idle;
			animation.CrossFade("Yellow_Rig|Yellow_Idle");
		}
	}

	void Jumping()
	{
		if ((!animation.isPlaying && TP_Controller.CharacterController.isGrounded) ||
			TP_Controller.CharacterController.isGrounded)
		{
			if (lastState == CharacterState.Running || lastState == CharacterState.Walking)
				animation.CrossFade("Yellow_Rig|Yellow_Run_Land");
			else
				animation.CrossFade("Yellow_Rig|Yellow_Jump_Land");
			State = CharacterState.Landing;
		}
		else if (!animation.IsPlaying("Yellow_Rig|Yellow_Jump"))
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
			animation.Stop();
			MoveDirection = Direction.Locked;
			animation.Play("Yellow_Rig|Yellow_Falling_Land");
			lastState = CharacterState.Falling;
			State = CharacterState.Landing;
		}
	}

	void Landing()
	{
		if (lastState == CharacterState.Running || lastState == CharacterState.Walking && !animation.isPlaying)
		{
			State = lastState;
		}
		else if (lastState == CharacterState.Falling && !animation.IsPlaying("Yellow_Rig|Yellow_Falling_Land"))
		{
			StartIdle();
		}
	}

	//void Sliding()
	//{
	//	if (!TP_Motor.Instance.IsSliding)
	//	{
	//		State = CharacterState.Idle;
	//		animation.CrossFade("Yellow_Rig|Yellow_Idle");
	//	}
	//}

	// is an animation playing? if not determine what animation should be playing or if we should end
	void Attacking()
	{
		if (//!animation.IsPlaying("Yellow_Rig|Yellow_Smash1") &&
			//!animation.IsPlaying("Yellow_Rig|Yellow_Smash2") &&
			//!animation.IsPlaying("Yellow_Rig|Yellow_Smash3") &&
			!animation.IsPlaying("Yellow_Rig|Yellow_Attack1") &&
			!animation.IsPlaying("Yellow_Rig|Yellow_Attack2") &&
			!animation.IsPlaying("Yellow_Rig|Yellow_Attack3"))
		{
			AttackAnimationStarted = false;
			//Debug.Log ("set to false");
		}

		if (!animation.isPlaying && IsAttacking && !EndAttack)
		{
			transform.rotation = Quaternion.Euler(transform.eulerAngles.x,Camera.main.transform.eulerAngles.y,transform.eulerAngles.z);
			//Debug.Log ("set to true");
			AttackAnimationStarted = true;
			if (IsSmashing)
			{
				switch (ComboCounter)
				{
					case 1:
						animation.CrossFade("Yellow_Rig|Yellow_Smash1");
				  		break;
					case 2:
						animation.CrossFade("Yellow_Rig|Yellow_Smash2");
						break;
					case 3:
						animation.CrossFade("Yellow_Rig|Yellow_Smash3");
						break;
				}
				IsAttacking = false;
			}
			else
			{
				ComboCounter++;
				switch (ComboCounter)
				{
					case 1:
						animation.Play("Yellow_Rig|Yellow_Attack1");
						break;
				case 2:
						animation.CrossFade("Yellow_Rig|Yellow_Attack2");
						break;
					case 3:
						animation.CrossFade("Yellow_Rig|Yellow_Attack3");
						break;
				}
				IsAttacking = false;
			}
		}
		else if (!animation.isPlaying && !IsAttacking && !EndAttack)
		{
			if (IsSmashing)
			{	
				switch (ComboCounter)
				{
					case 1:
						animation.CrossFade("Yellow_Rig|Yellow_ReturnSmash1");
						break;
					case 2:
						animation.CrossFade("Yellow_Rig|Yellow_ReturnSmash2");
						break;
					case 3:
						animation.CrossFade("Yellow_Rig|Yellow_ReturnSmash3");
						break;
				}
				EndAttack = true;
			}
			else
			{
				switch (ComboCounter)
				{
					case 1:
						animation.CrossFade("Yellow_Rig|Yellow_Return1");				
						break;
					case 2:
						animation.CrossFade("Yellow_Rig|Yellow_Return2");
						break;
					case 3:
						animation.CrossFade("Yellow_Rig|Yellow_Return3");
						break;
				}
				EndAttack = true;
			}
			animation.PlayQueued("Yellow_Rig|Yellow_Idle");
			EndAttack = false;
			ComboCounter = 0;
			IsSmashing = false;
		}
		else if (animation.IsPlaying("Yellow_Rig|Yellow_Idle"))
		{
			StartIdle();
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
	//		animation.Play("Yellow_Rig|Yellow_Idle");
	//		// put our charactercontroller back with the character after the animation
	//		var postClimbOffset = transform.TransformDirection(PostClimbOffset);
	//		transform.position = new Vector3(abdomen.position.x, climbPoint.position.y + climbPoint.localScale.y / 2, abdomen.position.z) + postClimbOffset;
	//	}
	//}

	void Defending()
	{
		if (IsDefending && animation.isPlaying)
		{
			animation.CrossFade("Yellow_Rig|Yellow_Defend");
			Debug.Log ("attackcounter = "+ComboCounter);
		}
		else if (!IsDefending && !animation.isPlaying)
		{
			StartIdle();
		}
	}

	void Dead()
	{

	}

	//'Start Action' methods below (called once per change of state)

	public void StartIdle()
	{
		ComboCounter = 0;
		State = CharacterState.Idle;
		MoveDirection = Direction.Stationary;
	}

	public void Use()
	{
		MoveDirection = Direction.Locked;
		lastState = State;
		State = CharacterState.Using;
		//animation.CrossFade();
	}

	public void Run()
	{
		if (MoveDirection == Direction.Locked)
			return;
		lastState = State;
		State = CharacterState.Running;
		MoveDirection = Direction.Forward;
		animation.CrossFade("Yellow_Rig|Yellow_Run");
	}

	public void Walk()
	{
		if (MoveDirection == Direction.Locked)
			return;
		lastState = State;
		animation.Stop();
		animation.Play("Yellow_Rig|Yellow_Walk");
		MoveDirection = Direction.Forward;
		State = CharacterState.Walking;
	}

	public void Jump()
	{
		if (MoveDirection == Direction.Locked)
			return;
		if (!TP_Controller.CharacterController.isGrounded || IsDead || State == CharacterState.Jumping || 
		    State == CharacterState.Attacking || State == CharacterState.Landing)
			return;

		lastState = State;
		State = CharacterState.Jumping;
		animation.CrossFade("Yellow_Rig|Yellow_Jump");
	}

	public void Fall()
	{
		if (IsDead)
			return;
		lastState = State;
		State = CharacterState.Falling;
		// if we are too high start a falling state immediately
		animation.CrossFade("Yellow_Rig|Yellow_Falling");
	}

	//public void Slide()
	//{
		//State = CharacterState.Sliding;
		//animation.CrossFade("Yellow_Rig|Yellow_Falling");
	//}

	public void Attack()
	{
		MoveDirection = Direction.Locked;
		if (State == CharacterState.Attacking && !EndAttack && !IsSmashing)
		{
			IsAttacking = true;
		}
		else if (State != CharacterState.Attacking)
		{
			animation.Stop();
			State = CharacterState.Attacking;
		}
	}

	public void SmashAttack()
	{
		IsAttacking = true;
		IsSmashing = true;
	}

	public void Defend()
	{
		MoveDirection = Direction.Locked;
		lastState = State;
		State = CharacterState.Defending;
		animation.CrossFade("Yellow_Rig|Yellow_Defend");
		IsDefending = true;
	}

	public void EndDefend()
	{
		animation.CrossFade("Yellow_Rig|Yellow_ReturnDefend");
		IsDefending = false;
		animation.PlayQueued("Yellow_Rig|Yellow_Idle");
		if (animation.IsPlaying("Yellow_Rig|Yellow_Idle"))
		{
			StartIdle();
		}

	}

	public void Die()
	{
		State = CharacterState.Dead;
		MoveDirection = Direction.Locked;
		IsDead = true;
		animation.Stop();
		animation.Play("Yellow_Rig|Yellow_Dead");
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
		HealthBar.Instance.ResetHealth();
		transform.position = initialPosition;
		transform.rotation = initialRotation;
		IsDead = false;
		StartIdle();
	}
}