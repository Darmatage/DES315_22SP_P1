using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomePlayerControllerScript : MonoBehaviour
{
	public float moveDistance;
	private Vector3 change; // player movement direction
	private Rigidbody2D rb2d;
	private Animator anim;
	private bool isAlive = true;

	private Renderer rend;

    // Start is called before the first frame update
    void Start(){
		anim = gameObject.GetComponentInChildren<Animator>();
		rend = GetComponentInChildren<Renderer> ();

		if (gameObject.GetComponent<Rigidbody2D>() != null) {
			rb2d = GetComponent<Rigidbody2D>();
		}
    }

	public void UpdateAnimationAndMove(Vector3 change) {
		if (isAlive == true){
			if (change!=Vector3.zero) {
				rb2d.MovePosition(transform.position + change * moveDistance);
				//MoveCharacter();
				//anim.SetFloat("moveX", change.x);
				//anim.SetFloat("moveY", change.y);
				//anim.SetBool("Walk", true);
			} else {
				//anim.SetBool("Walk", false);
			}
		}
	}

	public void playerHit(){
		if (isAlive == true){
			anim.SetTrigger("Hurt"); 
			StopCoroutine(ChangeColor());
			StartCoroutine(ChangeColor());
		}
	}

	public void playerDie(){
		anim.SetBool("isDead", true);
		if (isAlive == false) {
			//Debug.Log("I'm already dead");
		}
		else if (isAlive == true) {
			isAlive = false;
			gameObject.GetComponent<Collider2D>().enabled = false;
			//gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
		}
	}


	IEnumerator ChangeColor(){
		// color values are R, G, B, and alpha, each 0-255 divided by 100
		rend.material.color = new Color(2.0f, 1.0f, 0.0f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		rend.material.color = Color.white;
	}

}
