using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenMowry_PassiveRegenScript : MonoBehaviour
{
    public int healing = 1;
	public float healTime = 5.0f;
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
}
