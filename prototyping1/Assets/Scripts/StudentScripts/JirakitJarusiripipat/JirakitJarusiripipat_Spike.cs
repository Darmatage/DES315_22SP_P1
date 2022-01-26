using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JirakitJarusiripipat_Spike : MonoBehaviour
{
	public int damage = 10;
	private JirakitJarusiripipat_GameHandler gameHandlerObj;

	void Start()
	{
		GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
		if (gameHandlerLocation != null)
		{
			gameHandlerObj = gameHandlerLocation.GetComponent<JirakitJarusiripipat_GameHandler>();
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			gameHandlerObj.TakeDamage(damage);
		}
	}
}
