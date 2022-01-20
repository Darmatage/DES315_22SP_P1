using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JirakitJarusiripipat_SkullDetection : MonoBehaviour
{
    private JirakitJarusiripipat_ShootMove skull;
    private void Start()
    {
		skull = GetComponentInParent<JirakitJarusiripipat_ShootMove>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(skull.detectionTag))
		{
			skull.playerInArea = true;
			skull.player = collision.gameObject.transform;
		}
	}
}
