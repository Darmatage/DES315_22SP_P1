using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JirakitJarusiripipat_ShootMove : MonoBehaviour
{
	public float speed = 2f;
	public float defalutSpeed = 2f;
	public float stoppingDistance = 4f; // when enemy stops moving towards player
	public float retreatDistance = 3f; // when enemy moves away from approaching player
	private float timeBtwShots;
	public float startTimeBtwShots = 2;
	public GameObject projectile;

	private Animator anim;
	private Rigidbody2D rb;
	public Transform player;
	private Vector2 PlayerVect;

	public int EnemyLives = 30;
	private Renderer rend;
	private JirakitJarusiripipat_GameHandler gameHandlerObj;
	[SerializeField]
	private int knockbackPower;


	public bool playerInArea = false;
	public string detectionTag = "Player";
	void Start()
	{
		Physics2D.queriesStartInColliders = false;

		anim = GetComponentInChildren<Animator>();
		rend = GetComponentInChildren<Renderer>();
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		PlayerVect = player.transform.position;

		timeBtwShots = startTimeBtwShots;

		GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
		if (gameHandlerLocation != null)
		{
			gameHandlerObj = gameHandlerLocation.GetComponent<JirakitJarusiripipat_GameHandler>();
		}
	}

	void Update()
	{
		if (player != null && playerInArea)
		{

			// approach player
			if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
			{
				transform.position = Vector2.MoveTowards(transform.position, player.position, (speed / 2) * Time.deltaTime);
				anim.SetBool("Walk", true);
				Vector2 lookDir = PlayerVect - rb.position;
				//float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg -90f;
				//rb.rotation = angle;
			}

			// stop moving
			else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
			{
				transform.position = this.transform.position;
				anim.SetBool("Walk", false);
			}

			// retreat from player
			else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
			{
				transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
				anim.SetBool("Walk", true);
			}

			if (timeBtwShots <= 0)
			{
				anim.SetTrigger("Attack");
				Instantiate(projectile, transform.position, Quaternion.identity);
				timeBtwShots = startTimeBtwShots;
			}
			else
			{
				timeBtwShots -= Time.deltaTime;
			}
			if(player.GetComponent<JirakitJarusiripipat_PlayerAction>().isUsingSkill)
            {
				speed = 1;
            }
			else
            {
				speed = defalutSpeed;
            }
		}
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "bullet")
        //{
        //    EnemyLives -= 1;
        //    StopCoroutine("HitEnemy");
        //    StartCoroutine("HitEnemy");
        //}
        if (collision.gameObject.tag == "Player")
        {
            EnemyLives -= 2;
            StopCoroutine("HitEnemy");
            StartCoroutine("HitEnemy");
        }
    }
	
	IEnumerator HitEnemy()
    {
		// color values are R, G, B, and alpha, each divided by 100
		StartCoroutine(JirakitJarusiripipat_PlayerMove.instance.Knockback(knockbackPower, this.gameObject));
		StopCoroutine(JirakitJarusiripipat_PlayerMove.instance.Knockback(knockbackPower, this.gameObject));
		EnemyLives -= 1;
		rend.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);
        if (EnemyLives < 1)
        {
            //gameHandlerObj.AddScore(5);
            Destroy(gameObject);
        }
        else yield return new WaitForSeconds(0.5f);
        rend.material.color = Color.white;
    }
}