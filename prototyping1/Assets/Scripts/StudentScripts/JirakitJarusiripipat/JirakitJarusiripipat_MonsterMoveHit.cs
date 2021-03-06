using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JirakitJarusiripipat_MonsterMoveHit : MonoBehaviour
{
	public float speed = 4f;
	public float defaultSpeed = 4f;
	public Transform target;
	public int damage = 1;
	public float EnemyLives = 3;
	public float maxEnemyLives = 3;
	private Renderer rend;
	private JirakitJarusiripipat_GameHandler gameHandlerObj;
	private Animator anim;

	private float retreatTimer;
	public float retreatTime = 3.0f;
	private bool attackPlayer = true;

	public float knockbackPower;

	public bool playerInArea = false;
	public string detectionTag = "Player";
	private JirakitJarusiripipat_SFX SFX;

	[SerializeField]
	private Image healthFill;
	[SerializeField]
	JirakitJarusiripipat_GameManager gameManager;
	void Start()
	{
		anim = gameObject.GetComponentInChildren<Animator>();
		rend = GetComponentInChildren<Renderer>();
		SFX = GameObject.FindGameObjectWithTag("Respawn").GetComponent<JirakitJarusiripipat_SFX>();
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
		if (gameHandlerLocation != null)
		{
			gameHandlerObj = gameHandlerLocation.GetComponent<JirakitJarusiripipat_GameHandler>();
		}
		
	
	}

	void Update()
	{
		if (gameManager != null)
		{
			if (gameManager.gameStart)
            {
				//int playerHealth = GameHandler.PlayerHealth; //access script directly in the case of a static variable 
				if (target != null /*&& playerInArea*/)
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
					if (target.gameObject.GetComponent<JirakitJarusiripipat_PlayerAction>().isUsingSkill)
					{
						speed = 1;
					}
					else
					{
						speed = defaultSpeed;
					}
				}
				healthFill.fillAmount = EnemyLives / maxEnemyLives;
			}

		}
		else
        {

			//int playerHealth = GameHandler.PlayerHealth; //access script directly in the case of a static variable 
			if (target != null /*&& playerInArea*/)
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
				if (target.gameObject.GetComponent<JirakitJarusiripipat_PlayerAction>().isUsingSkill)
				{
					speed = 1;
				}
				else
				{
					speed = defaultSpeed;
				}
			}
			healthFill.fillAmount = EnemyLives / maxEnemyLives;
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
			gameHandlerObj.TakeDamage(damage);
			attackPlayer = false;

			//EnemyLives -= EnemyLives;
			//rend.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);
			//Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(detectionTag))
		{
			playerInArea = true;
			target = collision.gameObject.transform;
		}
	}
	

	IEnumerator GetHit()
	{
		StartCoroutine(JirakitJarusiripipat_PlayerMove.instance.Knockback(knockbackPower, this.gameObject));
		StopCoroutine(JirakitJarusiripipat_PlayerMove.instance.Knockback(knockbackPower, this.gameObject));

		anim.SetTrigger("Hurt");
		EnemyLives -= 1;
		SFX.Punch.Play();
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

