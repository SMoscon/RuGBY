using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
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

	public GameObject attackParticleSystem;

	// Use this for initialization
	void Start () {
		attackTimer = 0;
		cooldown = animation[attackAnimation1.name].length - 0.8f;
		smashCooldown = animation[attackAnimation1.name].length + 0.8f;
	}
	
	// Update is called once per frame
	void Update () {
		if (attackTimer > 0)
			attackTimer -= Time.deltaTime;
		if (attackTimer < 0)
			attackTimer = 0;
		if (Input.GetMouseButtonDown(0)) {
			//TP_Animator.Instance.Attacking();
			Debug.Log ("attacking = "+attacking);
			if (attackTimer == 0){
				if (attacking){
					if (attackSequence < 2){
						willAttack = true;
					}
				}
				else{
					Attack();
					attackTimer = cooldown;
				}
			}
		}
		if (Input.GetMouseButtonDown(1)){
			//Instantiate (attackParticleSystem, transform.position, transform.rotation);

			if (attacking){
				Debug.Log ("isplaying?");
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
		if (Input.GetKeyDown("e")){
			Instantiate (attackParticleSystem, transform.position + transform.up + transform.forward, transform.rotation);
			Debug.Log ("hellopressing E");
		}
		if (smashing){
			Debug.Log ("isplaying?");
			if (!animation.isPlaying){
				/*if (attackSequence == 0){
					animation.Play (returnSmashAnimation1.name);
				}*/
				if (attackSequence == 0){
					animation.Play (returnSmashAnimation1.name);
				}
				else if (attackSequence == 1){
					animation.Play (returnSmashAnimation2.name);
				}
				else if (attackSequence == 2){
					animation.Play (returnSmashAnimation3.name);
				}
				if (!animation.isPlaying){
					if (attackSequence == 0){
						animation.Play (returnAnimation1.name);
					}
					else if (attackSequence == 1){
						animation.Play (returnAnimation2.name);
					}
					else if (attackSequence == 2){
						animation.Play (returnAnimation3.name);
					}
					attacking = false;
					attackSequence = 0;
					Debug.Log ("return animation. attacking = false and sequence reset");
				}
				smashing = false;
				attacking = false;
				attackSequence = 0;
				//Debug.Log ("return animation. attacking = false and sequence reset");
			}
		}

		if (attacking){
			Debug.Log ("isplaying?");
			if (attackTimer == 0){
					Debug.Log ("isplaying?");
				if (!animation.isPlaying){
					Debug.Log ("return animation. attacking = false and sequence reset: "+attackSequence);
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
						Debug.Log ("return animation. attacking = false and sequence reset: "+attackSequence);
						if (attackSequence == 0){
							animation.Play (returnAnimation1.name);
						}
						else if (attackSequence == 1){
							animation.Play (returnAnimation2.name);
						}
						else if (attackSequence == 2){
							animation.Play (returnAnimation3.name);
						}
						attacking = false;
						attackSequence = 0;
						//Debug.Log ("return animation. attacking = false and sequence reset: "+attackSequence);
					}
				}
			}
		}
	}

	private void Attack() {
		float distance = Vector3.Distance(target.transform.position, transform.position);
		Vector3 dir = (target.transform.position - transform.position).normalized;
		float direction = Vector3.Dot (dir, transform.forward);
		//Debug.Log("boo");

		attacking = true;
		willAttack = false;
		if (attackSequence == 0){
			animation[attackAnimation1.name].speed = attackSpeed;
			animation.Play(attackAnimation1.name);

			if (distance < 2.5f && direction > 0){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-10);
			}
		}
		else if (attackSequence == 1){
			animation[attackAnimation2.name].speed = attackSpeed;
			animation.Play(attackAnimation2.name);
			if (distance < 3f && direction > 0){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-10);
			}
		}
		else if (attackSequence == 2){
			animation[attackAnimation3.name].speed = attackSpeed;
			animation.Play(attackAnimation3.name);
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

		Instantiate (attackParticleSystem, transform.position + transform.up + transform.forward, transform.rotation);
		attacking = true;
		smashing = true;
		willSmash = false;
		if (attackSequence == 0){
			animation[smashAnimation1.name].speed = attackSpeed;
			animation.Play(smashAnimation1.name);
			if (distance < 3f && direction > 0){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-10);
			}
		}
		else if (attackSequence == 1){
			animation[smashAnimation2.name].speed = attackSpeed;
			animation.Play(smashAnimation2.name);
			if (distance < 4f && direction > 0){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-20);
			}
		}
		else if (attackSequence == 2){
			animation[smashAnimation3.name].speed = attackSpeed;
			animation.Play(smashAnimation3.name);
			if (distance < 5f){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-15);
			}
		}
	}
	public void Defend () {
		defending = true;
		//animation[defendAnimation.name].speed = attackSpeed;
		animation.Play(defendAnimation.name);
	}
	public void ReleaseDef() {
		defending = false;
		//animation[returnDefendAnimation.name].speed = attackSpeed;
		animation.Play(returnDefendAnimation.name);
	}
}
