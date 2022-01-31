using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErinScribner_IceHeal : MonoBehaviour
{
	private int heal = 1;
	private float damageTime = 0.5f;
	private bool isHealing = false;
	private float damageTimer = 0f;
	private GameHandler gameHandlerObj;
	private ErinScribner_PaintTile paint;

	void Start()
	{

		if (GameObject.FindGameObjectWithTag("GameHandler") != null)
		{
			gameHandlerObj = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
		}
		GameObject check = GameObject.Find("ErinScribner_PlaceBlock");
		paint = check.GetComponent<ErinScribner_PaintTile>();
		heal = paint.heal;
		damageTime = paint.damageTime;
		
	}

	void FixedUpdate()
	{
		isHealing = paint.isHealing;
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
}
