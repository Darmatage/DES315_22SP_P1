using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonjuJo_PlayerMovement : MonoBehaviour
{

	public float speed = 3f; // player movement speed
	private Vector3 change; // player movement direction
	private Rigidbody2D rb2d;
	private Animator anim;
	private bool isAlive = true;
	private bool IsAttack = false;
	public GameObject Fireball;
	public Transform FireballPosition;

	private Renderer rend;

	Time Timer;
	float FireballCooldown = 1.5f;
	[SerializeField]
	private float NextFireballCooldown;

	// Start is called before the first frame update
	void Start()
	{
		anim = gameObject.GetComponentInChildren<Animator>();
		rend = GetComponentInChildren<Renderer>();

		if (gameObject.GetComponent<Rigidbody2D>() != null)
		{
			rb2d = GetComponent<Rigidbody2D>();
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{

		if (isAlive == true)
		{
			change = Vector3.zero;
			change.x = Input.GetAxisRaw("Horizontal");
			change.y = Input.GetAxisRaw("Vertical");
			UpdateAnimationAndMove();

			if (Input.GetAxis("Horizontal") > 0)
			{
				Vector3 newScale = transform.localScale;
				newScale.x = 1.0f;
				transform.localScale = newScale;
			}
			else if (Input.GetAxis("Horizontal") < 0)
			{
				Vector3 newScale = transform.localScale;
				newScale.x = -1.0f;
				transform.localScale = newScale;
			}

			if (Input.GetKey(KeyCode.Mouse0))
			{
				anim.SetTrigger("Attack");
				IsAttack = true;
			}

			if(Time.time > NextFireballCooldown)
            {
				if (Input.GetKey(KeyCode.Mouse1))
				{
					Instantiate(Fireball, FireballPosition.position, Quaternion.identity);
					NextFireballCooldown = Time.time + FireballCooldown;
				}
			}

		} //else playerDie(); //run this function from the GameHandler instead
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Enemy" && IsAttack)
		{
			WonjuJo_MonsterHandler MH = collision.gameObject.GetComponent<WonjuJo_MonsterHandler>();
			if (MH)
				MH.MonsterTakeDamge(5);
		}
	}

	void UpdateAnimationAndMove()
	{
		if (isAlive == true)
		{
			if (change != Vector3.zero && speed != 0)
			{
				rb2d.MovePosition(transform.position + change * speed * Time.deltaTime);
				//MoveCharacter();
				//anim.SetFloat("moveX", change.x);
				//anim.SetFloat("moveY", change.y);
				anim.SetBool("Walk", true);
			}
			else
			{
				anim.SetBool("Walk", false);
			}
		}
	}

	public void playerHit()
	{
		if (isAlive == true)
		{
			anim.SetTrigger("Hurt");
			StopCoroutine(ChangeColor());
			StartCoroutine(ChangeColor());
		}
	}

	public void playerDie()
	{
		anim.SetBool("isDead", true);
		if (isAlive == false)
		{
			//Debug.Log("I'm already dead");
		}
		else if (isAlive == true)
		{
			isAlive = false;
			gameObject.GetComponent<Collider2D>().enabled = false;
			//gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
		}
	}


	IEnumerator ChangeColor()
	{
		// color values are R, G, B, and alpha, each 0-255 divided by 100
		rend.material.color = new Color(2.0f, 1.0f, 0.0f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		rend.material.color = Color.white;
	}

	public bool GetIsAttack()
    {
		return IsAttack;
    }
}
