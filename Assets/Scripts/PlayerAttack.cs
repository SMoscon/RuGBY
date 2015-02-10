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

	public bool attacking = false;

	// Use this for initialization
	void Start () {
		attackTimer = 0;
		cooldown = 0.83f;
	}
	
	// Update is called once per frame
	void Update () {
		if (attackTimer > 0)
			attackTimer -= Time.deltaTime;
		if (attackTimer < 0)
			attackTimer = 0;
		if (Input.GetMouseButtonDown(0)){
			if (attackTimer == 0){
				Attack();
				attackTimer = cooldown;
			}
		}
		if (attacking){
			if (attackTimer == 0){
				attacking = false;
				animation.Play ("Yellow_Rig|Yellow_Attack2");
			}
		}

	}

	private void Attack() {
		float distance = Vector3.Distance(target.transform.position, transform.position);

		Vector3 dir = (target.transform.position - transform.position).normalized;

		float direction = Vector3.Dot (dir, transform.forward);

		Debug.Log (direction);
		if (distance < 2.5f && direction > 0){
			animation.Play("Yellow_Rig|Yellow_Attack1");
			attacking = true;
			EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
			eh.AdjustCurrentHealth(-10);
		}
	}
}
