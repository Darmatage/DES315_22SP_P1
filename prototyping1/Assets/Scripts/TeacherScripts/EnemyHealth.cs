using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour{
	
	public int EnemyLives = 3;
	private GameHandler gameHandlerObj;
	
	private Animator anim;
	private Renderer rend;
	
	public bool isStunned = false;
	public float stunTime = 5f;
	private float stuncounter =0f;
	
    void Start(){
		anim = gameObject.GetComponentInChildren<Animator>();
		rend = GetComponentInChildren<Renderer> ();
		stuncounter = stunTime;
    }

	void FixedUpdate(){
		if (isStunned == true){
			stuncounter -= 0.01f;
			if (stuncounter <= 0){
				stuncounter = stunTime;
                isStunned = false;
            }
        }
	}


    public void HitEnemy(){
		StopCoroutine("GetHit");
		StartCoroutine("GetHit");   
    }
	
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "bullet") {
			StopCoroutine("GetHit");
			StartCoroutine("GetHit");
		}
	}
	
	IEnumerator GetHit(){
		anim.SetTrigger("Hurt");
		EnemyLives -= 1;
		// color values are R, G, B, and alpha, each divided by 100
		rend.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);
		if (EnemyLives < 1){
			//gameHandlerObj.AddScore (1);
			Destroy(gameObject);
		}
		else yield return new WaitForSeconds(0.5f);
		rend.material.color = Color.white;
	}
	
}
