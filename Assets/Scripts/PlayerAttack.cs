using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	public GameObject target;
	public float attackTimer;
	public float cooldown;

	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	public AnimationClip attackAnimation1;
	public float attackSpeed = 3.0F;
	public bool attacking = false;
	public AnimationClip attackAnimation2;
	public AnimationClip attackAnimation3;
	public AnimationClip smashAnimation;
	public AnimationClip returnAnimation1;
	public AnimationClip returnAnimation2;
	public AnimationClip returnAnimation3;
	public AnimationClip returnAnimation4;

	private int attackSequence = 0;

	// Use this for initialization
	void Start () {
		attackTimer = 0;
		cooldown = animation[attackAnimation1.name].length - 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		if (attackTimer > 0)
			attackTimer -= Time.deltaTime;
		if (attackTimer < 0)
			attackTimer = 0;
		if (Input.GetMouseButtonDown(0)){
			if (attackTimer == 0){
				if (attacking){
					if (attackSequence < 4){
						attackSequence++;
						Attack();
						attackTimer = cooldown;
					}
					else
						return;
				}
				else{
					Attack();
					attackTimer = cooldown;
				}
			}
		}
		if (attacking){
			if (attackTimer == 0){
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
					else if (attackSequence == 3){
						animation.Play (returnAnimation4.name);
					}
					attacking = false;
					attackSequence = 0;
					Debug.Log ("return animation. attacking = false and sequence reset");
				}
			}

		}
		else {
			//Debug.Log("Doing nothing bro");
		}

	}

	private void Attack() {
		float distance = Vector3.Distance(target.transform.position, transform.position);
		Vector3 dir = (target.transform.position - transform.position).normalized;
		float direction = Vector3.Dot (dir, transform.forward);

		attacking = true;
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
		else if (attackSequence == 3){
			animation[smashAnimation.name].speed = attackSpeed;
			animation.Play(smashAnimation.name);
			if (distance < 5f && direction > 0){
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				eh.AdjustCurrentHealth(-20);
			}
		}
		//else 
			//Debug.Log ("SMASHING");
		/*if (distance < 2.5f && direction > 0){
			EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
			eh.AdjustCurrentHealth(-10);
		}*/
	}
}
