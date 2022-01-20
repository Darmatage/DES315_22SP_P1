using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JirakitJarusiripipat_SlimeDetection : MonoBehaviour
{
	private JirakitJarusiripipat_MonsterMoveHit slime;
	private void Start()
	{
		slime = GetComponentInParent<JirakitJarusiripipat_MonsterMoveHit>();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(slime.detectionTag))
		{
			slime.playerInArea = true;
			slime.target = collision.gameObject.transform;
		}
	}
}
