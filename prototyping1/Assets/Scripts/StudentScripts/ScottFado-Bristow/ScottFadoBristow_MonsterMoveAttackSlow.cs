using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScottFadoBristow_MonsterMoveAttackSlow : MonoBehaviour
{
	//A chunk of this code is copied from MonsterMoveHit, used as a base
	public float speed = 4f;
	private Transform target;
	public float attackSlow = 1;
	public int EnemyLives = 3;
	private Renderer rend;
	private GameHandler gameHandlerObj;
	private Animator anim;

	//The lowest value the speed can get to
	private const float speedFloor = 1.0f;

	private float retreatTimer;
	public float retreatTime = 3.0f;
	private bool attackPlayer = true;

	void Start()
	{
		anim = gameObject.GetComponentInChildren<Animator>();
		rend = GetComponentInChildren<Renderer>();

		if (GameObject.FindGameObjectWithTag("Player") != null)
		{
			target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		}
		GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
		if (gameHandlerLocation != null)
		{
			gameHandlerObj = gameHandlerLocation.GetComponent<GameHandler>();
		}
	}

	void Update()
	{
		//int playerHealth = GameHandler.PlayerHealth; //access script directly in the case of a static variable 
		if (target != null)
		{
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
			//When colliding with the player, we want to apply slow to them
			//Gets the players move script
			PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();

			player.speed = Mathf.Max(speedFloor, player.speed - attackSlow);


			//So now we need to attatch a small slime to the players feet

			ScottFadoBristow_SlimeBoots muddyBoots = collision.gameObject.GetComponent<ScottFadoBristow_SlimeBoots>();
			if(muddyBoots)
            {
				muddyBoots.Attatch();
				//Destroy(transform.parent);
				Destroy(gameObject);
            }

			//This keeps the slime from baraging the player, wont need soon
			attackPlayer = false;

			//EnemyLives -= EnemyLives;
			//rend.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);
			//Destroy(gameObject);
		}
	}

	IEnumerator GetHit()
	{
		anim.SetTrigger("Hurt");
		EnemyLives -= 1;
		// color values are R, G, B, and alpha, each divided by 100
		rend.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);
		if (EnemyLives < 1)
		{
			//gameHandlerObj.AddScore (1);
			Destroy(gameObject);
		}
		else yield return new WaitForSeconds(0.5f);
		rend.material.color = Color.white;
	}
}
