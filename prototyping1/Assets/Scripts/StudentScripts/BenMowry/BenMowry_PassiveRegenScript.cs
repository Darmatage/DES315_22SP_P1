using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenMowry_PassiveRegenScript : MonoBehaviour
{
  public int healing = 1;
	public float safeTime = 3.0f;
	private float currTime = 0.0f;
	private bool isHealing = false;
	private int pastHealth = 100;
	private float healDelay = 0.5f;
	private float delayTime = 0.0f;
	private GameHandler gameHandlerObj;

	void Start () {
		if (GameObject.FindGameObjectWithTag ("GameHandler") != null) {
			gameHandlerObj = GameObject.FindGameObjectWithTag ("GameHandler").GetComponent<GameHandler>();
		}
		pastHealth = GameHandler.PlayerHealth;
	}

	void Update() {
		if(isHealing == false)
			currTime += Time.deltaTime;

		if(isHealing == true)
			delayTime += Time.deltaTime;
		
		if(currTime >= safeTime)
			isHealing = true;
		
		if(GameHandler.PlayerHealth < pastHealth) {
			currTime = 0.0f;
			isHealing = false;
			pastHealth = GameHandler.PlayerHealth;
		}
		
		if (isHealing == true && delayTime >= healDelay) {
			gameHandlerObj.Heal(healing);
			pastHealth += healing;
			delayTime = 0.0f;
		}

		if(pastHealth > gameHandlerObj.PlayerHealthStart)
		pastHealth = gameHandlerObj.PlayerHealthStart;
	}
}
