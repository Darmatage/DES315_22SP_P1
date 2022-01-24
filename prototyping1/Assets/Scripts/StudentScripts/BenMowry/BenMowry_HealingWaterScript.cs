using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenMowry_HealingWaterScript : MonoBehaviour
{
  public int healing = 1;
	public float healTime = 0.5f;
	private bool isHealing = false;
	private float healTimer = 0f;
	private GameHandler gameHandlerObj;

	void Start () {

		if (GameObject.FindGameObjectWithTag ("GameHandler") != null) {
			gameHandlerObj = GameObject.FindGameObjectWithTag ("GameHandler").GetComponent<GameHandler>();
		}
	}

	void FixedUpdate(){
		if (isHealing == true){
			healTimer += 0.1f;
			if (healTimer >= healTime){
				gameHandlerObj.Heal(healing);
				healTimer = 0f;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			isHealing = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			isHealing = false;
		}
	}
}
