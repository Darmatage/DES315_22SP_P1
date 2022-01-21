using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErinScribner_IceHeal : MonoBehaviour
{
	public int heal = 1;
	public float damageTime = 0.5f;
	private bool isHealing = false;
	private float damageTimer = 0f;
	private GameHandler gameHandlerObj;


	void Start()
	{

		if (GameObject.FindGameObjectWithTag("GameHandler") != null)
		{
			gameHandlerObj = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
		}
	}

	void FixedUpdate()
	{
		if (isHealing == true)
		{
			damageTimer += 0.1f;
			if (damageTimer >= damageTime)
			{
				gameHandlerObj.Heal(heal);
				damageTimer = 0f;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{

		if (other.gameObject.tag == "Player")
		{
			isHealing = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			isHealing = false;
		}
	}

}
