using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveHit : MonoBehaviour{
	public float speed = 4f;
	public int damage = 1;
	private Transform target;
	public float attackRange = 10f; // threshold distance for enemy to attack player

	private float retreatTimer;
	public float retreatTime = 3.0f;
	private bool attackPlayer = true;

	//public int EnemyLives = 3;
	//private Renderer rend;
	private Animator anim;
	private GameHandler gameHandlerObj;
	private bool isStunned = false; //access isStunned through EnemyHealth.cs

	void Start () {
		anim = gameObject.GetComponentInChildren<Animator>();
		//rend = GetComponentInChildren<Renderer> ();

		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		}
		GameObject gameHandlerLocation = GameObject.FindWithTag ("GameHandler");
		if (gameHandlerLocation != null) {
			gameHandlerObj = gameHandlerLocation.GetComponent<GameHandler> ();
		}
	}

	void Update () {
		float DistToPlayer = Vector2.Distance(transform.position, target.position);
		
		//int playerHealth = GameHandler.PlayerHealth; //access script directly in the case of a static variable 
		isStunned = gameObject.GetComponent<EnemyHealth>().isStunned;		
		if ((target != null) && (isStunned == false) && (DistToPlayer <= attackRange)){
			//if ((attackPlayer == true) && (playerHealth >= 1)){
			if (attackPlayer == true){
				transform.position = Vector2.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
			} else if (attackPlayer == false){
				transform.position = Vector2.MoveTowards (transform.position, target.position, speed * -1 * Time.deltaTime);}
		}
	}

	void FixedUpdate(){
		retreatTimer += 0.1f;
		if (retreatTimer >= retreatTime){
			attackPlayer = true;
			retreatTimer = 0f;
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.tag == "Player") {
			gameHandlerObj.TakeDamage (damage);
			attackPlayer = false;

			//EnemyLives -= EnemyLives;
			//rend.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);
			//Destroy(gameObject);
		}
		// else if (collision.gameObject.tag == "bullet") {
			// StopCoroutine("GetHit");
			// StartCoroutine("GetHit");
		// }
	}
	
	//DISPLAY the range of enemy's attack when selected in the Editor
    void OnDrawGizmosSelected(){
              Gizmos.DrawWireSphere(transform.position, attackRange);
    }
	
}
