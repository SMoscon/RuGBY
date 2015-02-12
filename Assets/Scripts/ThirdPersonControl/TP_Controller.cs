using UnityEngine;
using System.Collections;

public class TP_Controller : MonoBehaviour
{
	public static CharacterController CharacterController;
	public static TP_Controller Instance;

	public GameObject target;
	public float attackTimer;
	public float cooldown;
	public float smashCooldown;
	
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	public float attackSpeed = 3.0F;
	
	public AnimationClip attackAnimation1;
	public AnimationClip attackAnimation2;
	public AnimationClip attackAnimation3;
	public AnimationClip smashAnimation1;
	public AnimationClip smashAnimation2;
	public AnimationClip smashAnimation3;
	public AnimationClip returnAnimation1;
	public AnimationClip returnAnimation2;
	public AnimationClip returnAnimation3;
	public AnimationClip returnSmashAnimation1;
	public AnimationClip returnSmashAnimation2;
	public AnimationClip returnSmashAnimation3;
	public AnimationClip defendAnimation;
	public AnimationClip returnDefendAnimation;
	
	private bool attacking = false;
	private bool smashing = false;
	private bool defending = false;
	private int attackSequence = 0;
	private bool willAttack = false;
	private bool willSmash = false;

	public GameObject attackParticleSystem1;
	public GameObject attackParticleSystem2;
	public GameObject attackParticleSystem3;

	public bool ClimbEnabled { get; set; }
	
	void Start()
	{
		attackTimer = 0;
		cooldown = animation[attackAnimation1.name].length - 0.8f;
		smashCooldown = animation[attackAnimation1.name].length + 0.8f;
	}

	void Awake() 
	{
		CharacterController = GetComponent("CharacterController") as CharacterController;
		Instance = this;
	}
	
	void Update() 
	{
		if (Camera.main == null)
			return;
		
		GetLocomotionInput();
		HandleActionInput();
		
		TP_Motor.Instance.UpdateMotor();

		//Smash Attack particles
		var animRatio = animation[smashAnimation1.name].time % animation[smashAnimation1.name].length;
		if (animRatio > 0.3f && animRatio < 0.31f){
			Instantiate (attackParticleSystem1, transform.position + transform.up + transform.forward, Quaternion.identity);
		}
		animRatio = animation[smashAnimation2.name].time % animation[smashAnimation2.name].length;
		if (animRatio > 0.6f && animRatio < 0.7f){
			Instantiate (attackParticleSystem2, transform.position + transform.forward * 2, Quaternion.Euler(-90, 0, 0));
		}
		animRatio = animation[smashAnimation3.name].time % animation[smashAnimation3.name].length;
		if (animRatio > 0.66f && animRatio < 0.8f){
			Instantiate (attackParticleSystem3, transform.position + transform.up + transform.forward, Quaternion.identity);
			Instantiate (attackParticleSystem1, transform.position + transform.up + transform.forward + Vector3.left, Quaternion.identity);
			Instantiate (attackParticleSystem1, transform.position + transform.up + transform.forward + Vector3.right, Quaternion.identity);
		}
		
	}
	
	void GetLocomotionInput()
	{
		var deadzone = 0.1f;
		
		TP_Motor.Instance.VerticalVelocity = TP_Motor.Instance.MoveVector.y;
		TP_Motor.Instance.MoveVector = Vector3.zero;
		
		if (Input.GetAxis("Vertical") > deadzone || Input.GetAxis("Vertical") < -deadzone)
			TP_Motor.Instance.MoveVector += new Vector3(0, 0, Input.GetAxis("Vertical"));
		
		if (Input.GetAxis("Horizontal") > deadzone || Input.GetAxis("Horizontal") < -deadzone)
			TP_Motor.Instance.MoveVector += new Vector3(Input.GetAxis("Horizontal"),0, 0);
		
		TP_Animator.Instance.DetermineCurrentMoveDirection();
	}
	
	void HandleActionInput()
	{
		if (Input.GetButton("Jump"))
		{
			if (ClimbEnabled)
				Climb();
			else
				Jump();
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			Use();
		}

		if (attackTimer > 0)
			attackTimer -= Time.deltaTime;
		if (attackTimer < 0)
			attackTimer = 0;
		if (Input.GetMouseButtonDown(0)) {
			if (attackTimer == 0){
				if (attacking){
					if (attackSequence < 2){
						willAttack = true;
					}
				}
				else{
					if (!defending){
						Attack();
						attackTimer = cooldown;
					}
				}
			}
		}
		if (Input.GetMouseButtonDown(1)){
			
			if (attacking){
				if (attackTimer == 0)
					willSmash = true;
			}
			else{
				if (attackTimer == 0){
					if (!defending){
						Defend();
						//attackTimer = smashCooldown;
					}
				}
			}
		}
		if (Input.GetMouseButtonUp(1)){
			if (defending){
				ReleaseDef();
			}
		}
		if (smashing){
			if (!animation.isPlaying){
				if (attackSequence == 0){
					animation.CrossFade (returnSmashAnimation1.name);
				}
				else if (attackSequence == 1){
					animation.CrossFade (returnSmashAnimation2.name);
				}
				else if (attackSequence == 2){
					animation.CrossFade (returnSmashAnimation3.name);
				}
				if (!animation.isPlaying){
					if (attackSequence == 0){
						animation.CrossFade (returnAnimation1.name);
					}
					else if (attackSequence == 1){
						animation.CrossFade (returnAnimation2.name);
					}
					else if (attackSequence == 2){
						animation.CrossFade (returnAnimation3.name);
					}
					attacking = false;
					attackSequence = 0;
					Debug.Log ("return animation. attacking = false and sequence reset");
				}
				smashing = false;
				attacking = false;
				attackSequence = 0;
			}
		}
		
		if (attacking){
			if (attackTimer == 0){
				if (!animation.isPlaying){
					if (willSmash){
						willAttack = false;
						SmashAttack ();
						attackTimer = smashCooldown;
					}
					else if (willAttack && attackSequence < 2){
						attackSequence++;
						Attack ();
						attackTimer = cooldown;
					}
					else {
						if (attackSequence == 0){
							animation.CrossFade (returnAnimation1.name);
						}
						else if (attackSequence == 1){
							animation.CrossFade (returnAnimation2.name);
						}
						else if (attackSequence == 2){
							animation.CrossFade (returnAnimation3.name);
						}
						attacking = false;
						attackSequence = 0;
					}
				}
			}
		}

	}

	void Use()
	{
		TP_Animator.Instance.Use();
	}
	
	void Jump()
	{
		TP_Motor.Instance.Jump();
		TP_Animator.Instance.Jump();
	}

	void Climb()
	{

	}

	private void Attack() {
		float distance = Vector3.Distance(target.transform.position, transform.position);
		Vector3 dir = (target.transform.position - transform.position).normalized;
		float direction = Vector3.Dot (dir, transform.forward);
		
		attacking = true;
		willAttack = false;
		if (attackSequence == 0){
			animation[attackAnimation1.name].speed = attackSpeed;
			animation.CrossFade(attackAnimation1.name);
			
			if (distance < 2.7f && direction > 0){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-10);
			}
		}
		else if (attackSequence == 1){
			animation[attackAnimation2.name].speed = attackSpeed;
			animation.CrossFade(attackAnimation2.name);
			if (distance < 3f && direction > 0){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-10);
			}
		}
		else if (attackSequence == 2){
			animation[attackAnimation3.name].speed = attackSpeed;
			animation.CrossFade(attackAnimation3.name);
			if (distance < 4f){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-15);
			}
		}
	}
	private void SmashAttack() {
		float distance = Vector3.Distance(target.transform.position, transform.position);
		Vector3 dir = (target.transform.position - transform.position).normalized;
		float direction = Vector3.Dot (dir, transform.forward);
		
		attacking = true;
		smashing = true;
		willSmash = false;
		if (attackSequence == 0){
			animation[smashAnimation1.name].speed = attackSpeed;
			animation.CrossFade(smashAnimation1.name);
			if (distance < 3.2f && direction > 0){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-12);
			}
		}
		else if (attackSequence == 1){
			animation[smashAnimation2.name].speed = attackSpeed;
			animation.CrossFade(smashAnimation2.name);
			if (distance < 4f && direction > 0){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-20);
			}
		}
		else if (attackSequence == 2){
			animation[smashAnimation3.name].speed = attackSpeed;
			animation.CrossFade(smashAnimation3.name);
			if (distance < 4f && direction > 0){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-30);
			}
		}
	}
	public void Defend () {
		defending = true;
		//animation[defendAnimation.name].speed = attackSpeed;
		animation.CrossFade(defendAnimation.name);
	}
	public void ReleaseDef() {
		defending = false;
		//animation[returnDefendAnimation.name].speed = attackSpeed;
		animation.CrossFade(returnDefendAnimation.name);
	}

	//void Attack(int attackNumber)
	//{
	//	TP_Animator.Instance.Attack(attackNumber);
	//}


	//void SmashAttack()
	//{

	//}

	//void Defend()
	//{

	//}
}