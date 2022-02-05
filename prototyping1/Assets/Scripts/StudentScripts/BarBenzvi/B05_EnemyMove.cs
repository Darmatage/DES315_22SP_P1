//This script is a copy paste of MonsterMoveHit with the damage removed from it so it doesn't trigger animations even if dealing 0 damage

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_EnemyMove : MonoBehaviour
{

	public float speed = 4f;
	private Transform target;
	public int EnemyLives = 3;
	private Renderer rend;
	private Animator anim;

	private float retreatTimer;
	public float retreatTime = 3.0f;
	private bool attackPlayer = true;

	public float StunTime = 3.0f;
	bool stunned = false;

	float stunTimer = 0.0f;

	public float AggroRange = 10.0f;
	public bool UsesAggroRange = false;

	void Start()
	{
		anim = gameObject.GetComponentInChildren<Animator>();
		rend = GetComponentInChildren<Renderer>();

		if (GameObject.FindGameObjectWithTag("Player") != null)
		{
			target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		}
	}

	void Update()
	{
		if(stunned)
        {
			stunTimer -= Time.deltaTime;
			if(stunTimer <= 0.0f)
            {
				stunned = false;
            }
			return;
        }

		//int playerHealth = GameHandler.PlayerHealth; //access script directly in the case of a static variable 
		if (target != null)
		{
			if(UsesAggroRange && Vector2.Distance(target.transform.position, transform.position) > AggroRange)
            {
				return;
            }

			//if ((attackPlayer == true) && (playerHealth >= 1)){
			if (attackPlayer == true)
			{
				transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
			}
			else if (attackPlayer == false)
			{
				transform.position = Vector2.MoveTowards(transform.position, target.position, speed * -1 * Time.deltaTime);
			}
		}
	}

	void FixedUpdate()
	{
		retreatTimer += 0.1f;
		if (retreatTimer >= retreatTime)
		{
			attackPlayer = true;
			retreatTimer = 0f;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "bullet")
		{
			StopCoroutine("GetHit");
			StartCoroutine("GetHit");
		}
		else if (collision.gameObject.tag == "Player")
		{
			attackPlayer = false;

			//EnemyLives -= EnemyLives;
			//rend.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);
			//Destroy(gameObject);
		}
	}

	public void HitEnemy()
    {
		EnemyLives -= 1;

		stunned = true;
		stunTimer = StunTime;

		if (EnemyLives < 1)
		{
			//gameHandlerObj.AddScore (1);
			Destroy(gameObject);
		}
		else
        {
			StopCoroutine("GetHit");
			StartCoroutine("GetHit");
		}	}

	IEnumerator GetHit()
	{
		anim.SetTrigger("Hurt");
		// color values are R, G, B, and alpha, each divided by 100
		rend.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);

		yield return new WaitForSeconds(0.5f);
		rend.material.color = Color.white;
	}


}
