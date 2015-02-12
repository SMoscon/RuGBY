using UnityEngine;
using System.Collections;

public class TP_Animator : MonoBehaviour {
	
	public AnimationClip idleAnimation;
	public AnimationClip walkforwardAnimation;
	public AnimationClip walkbackwardsAnimation;
	public AnimationClip strafeleftAnimation;
	public AnimationClip straferightAnimation;

	public enum Direction
	{
		Stationary, Forward, Backward, Left, Right, 
		LeftForward, RightForward, LeftBackward, RightBackward
	}

	public enum CharacterState
	{
		Idle, Walking, Running, WalkingBackwards, StrafingLeft, StrafingRight,
		Dodging, Falling, Landing, Climbing, Sliding, Attacking, Defending
		Dead, ActionLocked
	}
	
	public static TP_Animator Instance;
	
	public Direction MoveDirection { get; set; }
	public CharacterState State { get; set; }
	

	void Awake () 
	{
		Instance = this;
	}

	void Update()
	{
		Debug.Log("Current MoveState: " + MoveDirection.ToString());
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
			    State != CharacterState.Landing)
			{
				// We should be falling here
			}
		}
		if 	(State != CharacterState.Falling && 
			State != CharacterState.Landing &&
			State != CharacterState.Climbing &&
			State != CharacterState.Attacking &&
			State != CharacterState.Shielding &&
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
		}
	}

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
}