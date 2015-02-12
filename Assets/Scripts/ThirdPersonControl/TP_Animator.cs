using UnityEngine;
using System.Collections;

public class TP_Animator : MonoBehaviour 
{
	
	public AnimationClip idleAnimation;
	public AnimationClip walkforwardAnimation;
	public AnimationClip runAnimation;
	public AnimationClip walkbackwardsAnimation;
	public AnimationClip strafeleftAnimation;
	public AnimationClip straferightAnimation;
	public AnimationClip jumpAnimation;
	public AnimationClip runlandingAnimation;
	public AnimationClip usingAnimation;
	public AnimationClip landingAnimation;
	public AnimationClip fallingAnimation;

	public enum Direction
	{
		Stationary, Forward, Backward, Left, Right, 
		LeftForward, RightForward, LeftBackward, RightBackward
	}

	public enum CharacterState
	{
		Idle, Walking, Running, WalkingBackwards, StrafingLeft, StrafingRight,
		Dodging, Falling, Landing, Jumping, Attacking, Using, Defending,
		Dead, Climbing, Sliding
	}
	
	public static TP_Animator Instance;

	private CharacterState lastState;
	private Transform climbPoint;

	public Direction MoveDirection { get; set; }
	public CharacterState State { get; set; }
	public bool IsDead { get; set; }

	

	void Awake () 
	{
		Instance = this;
	}

	void Update()
	{
		DetermineCurrentState ();
		ProcessCurrentState ();
		Debug.Log("Current State: " + State.ToString());
	}

	
	public void DetermineCurrentMoveDirection()
	{
		var forward = false;
		var backward = false;
		var right = false;
		var left = false;
		
		
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
			State != CharacterState.Climbing &&
			State != CharacterState.Sliding) 
		{
			Debug.Log("If statement marker");
			switch (MoveDirection)
			{
				case Direction.Stationary:
					State = CharacterState.Idle;
					break;
				case Direction.Forward:
					State = CharacterState.Walking;
					break;
				case Direction.Backward:
					State = CharacterState.WalkingBackwards;
					break;
				case Direction.Left:
					State = CharacterState.StrafingLeft;
					break;
				case Direction.Right:
					State = CharacterState.StrafingRight;
					break;
				case Direction.LeftForward:
					State = CharacterState.Walking;
					break;
				case Direction.RightForward:
					State = CharacterState.Walking;
					break;
				case Direction.LeftBackward:
					State = CharacterState.WalkingBackwards;
					break;
				case Direction.RightBackward:
					State = CharacterState.WalkingBackwards;
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
				Debug.Log ("IDLE happens");
				Idle();
				break;
			case CharacterState.Walking:
				Walking();
				break;
			case CharacterState.WalkingBackwards:
				WalkingBackwards();
				break;
			case CharacterState.StrafingLeft:
				StrafingLeft();
				break;
			case CharacterState.StrafingRight:
				StrafingRight();
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
			case CharacterState.Sliding:
				Sliding();
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
		animation.CrossFade(walkforwardAnimation.name);
	}

	void Running()
	{
		animation.CrossFade(runAnimation.name);
	}

	void WalkingBackwards()
	{
		animation.CrossFade(walkbackwardsAnimation.name);
	}

	void StrafingLeft()
	{
		animation.CrossFade(strafeleftAnimation.name);
	}

	void StrafingRight()
	{
		animation.CrossFade(straferightAnimation.name);
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
			if (lastState == CharacterState.Running)
				animation.CrossFade(runlandingAnimation.name);
			else
				animation.CrossFade(landingAnimation.name);
			State = CharacterState.Landing;
		}
		else if (!animation.IsPlaying(jumpAnimation.name))
		{
			State = CharacterState.Falling;
			animation.CrossFade(fallingAnimation.name);
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
			if (lastState == CharacterState.Running)
				animation.CrossFade(runlandingAnimation.name);
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
				animation.Play(runAnimation.name);
			}	
		}
		else
		{
			if (!animation.IsPlaying(landingAnimation.name))
			{
				State = CharacterState.Idle;
				animation.Play(idleAnimation.name);
			}
		}
	}

	void Sliding()
	{
		if (!TP_Motor.Instance.IsSliding)
		{
			State = CharacterState.Idle;
			animation.CrossFade(idleAnimation.name);
		}
	}

	//'Start Action' methods below (called once per change of state)

	public void Use()
	{
		State = CharacterState.Using;
		animation.CrossFade(usingAnimation.name);
	}

	public void Jump()
	{
		if (!TP_Controller.CharacterController.isGrounded || IsDead || 
			State == CharacterState.Jumping)
			return;

		lastState = State;
		State = CharacterState.Jumping;
		animation.CrossFade(jumpAnimation.name);
	}

	public void Fall()
	{
		if (IsDead)
			return;

		lastState = State;
		State = CharacterState.Falling;
		// if we are too high start a falling state immediately
		animation.CrossFade(fallingAnimation.name);
	}

	public void Slide()
	{
		State = CharacterState.Sliding;
		animation.CrossFade(fallingAnimation.name);
	}

	public void SetClimbPoint(Transform climbPoint)
	{
		this.climbPoint = climbPoint;
		TP_Controller.Instance.ClimbEnabled = true;
	}

	public void ClearClimbPoint(Transform climbPoint)
	{

	}
}