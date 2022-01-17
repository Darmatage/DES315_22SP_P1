using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JirakitJarusiripipat_PlayerMove : MonoBehaviour
{
	public float speed = 3f; // player movement speed
	private Vector3 change; // player movement direction
	private Rigidbody2D rb2d;
	[HideInInspector]
	public Animator anim;
	private bool isAlive = true;

	public static JirakitJarusiripipat_PlayerMove instance;
	[HideInInspector]
	public JirakitJarusiripipat_PlayerAction playerAction;
	private Renderer rend;

	// Start is called before the first frame update
	void Start()
	{
		instance = this;
		anim = gameObject.GetComponentInChildren<Animator>();
		rend = GetComponentInChildren<Renderer>();

		if (gameObject.GetComponent<Rigidbody2D>() != null)
		{
			rb2d = GetComponent<Rigidbody2D>();
		}
		playerAction = GetComponent<JirakitJarusiripipat_PlayerAction>();
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

			//if (Input.GetKey(KeyCode.Space))
			//{
			//	anim.SetTrigger("Attack");
			//}
		} //else playerDie(); //run this function from the GameHandler instead
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

	public IEnumerator Knockback(float knockbackPower, GameObject obj)
    {

		//while(knockbackDuration > countdown)
  //      {
			//countdown += Time.deltaTime;
			Vector2 direction = (this.transform.position - obj.transform.position).normalized;
			obj.GetComponent<Rigidbody2D>().AddForce(-direction * knockbackPower);
        //}
		yield return null;
    }
	IEnumerator ChangeColor()
	{
		// color values are R, G, B, and alpha, each 0-255 divided by 100
		rend.material.color = new Color(2.0f, 1.0f, 0.0f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		rend.material.color = Color.white;
	}
}
