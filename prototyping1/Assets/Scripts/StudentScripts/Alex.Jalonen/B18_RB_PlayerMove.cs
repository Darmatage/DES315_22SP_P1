using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B18_RB_PlayerMove : PlayerMove
{
	private Rigidbody2D rb2d_;
	private Animator anim_;
	private bool isAlive_ = true;

	private Renderer rend_;

	protected new void Start()
	{
		anim_ = gameObject.GetComponentInChildren<Animator>();
		rend_ = GetComponentInChildren<Renderer>();

		if (gameObject.GetComponent<Rigidbody2D>() != null)
		{
			rb2d_ = GetComponent<Rigidbody2D>();
		}
	}

	protected override void FixedUpdate()
	{

		if (isAlive_ == true)
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

			if (Input.GetKey(KeyCode.Space))
			{
				anim_.SetTrigger("Attack");
			}
		} //else playerDie(); //run this function from the GameHandler instead
	}

	protected override void UpdateAnimationAndMove()
	{
		if (isAlive_ == true)
		{
			if (change != Vector3.zero && speed != 0)
			{
				rb2d_.velocity += (Vector2)change * speed * Time.deltaTime;
				//MoveCharacter();
				//anim.SetFloat("moveX", change.x);
				//anim.SetFloat("moveY", change.y);
				anim_.SetBool("Walk", true);
			}
			else
			{
				anim_.SetBool("Walk", false);
			}
		}
	}

	public override void playerHit()
	{
		if (isAlive_ == true)
		{
			anim_.SetTrigger("Hurt");
			StopCoroutine(ChangeColor());
			StartCoroutine(ChangeColor());
		}
	}

	public override void playerDie()
	{
		anim_.SetBool("isDead", true);
		if (isAlive_ == false)
		{
			//Debug.Log("I'm already dead");
		}
		else if (isAlive_ == true)
		{
			isAlive_ = false;
			gameObject.GetComponent<Collider2D>().enabled = false;
			//gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
		}
	}

	protected override IEnumerator ChangeColor()
	{
		// color values are R, G, B, and alpha, each 0-255 divided by 100
		rend_.material.color = new Color(2.0f, 1.0f, 0.0f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		rend_.material.color = Color.white;
	}
}
