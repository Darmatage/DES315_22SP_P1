using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillKickedEnemy : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.CompareTag("Kicked"))
            {
                collision.gameObject.GetComponent<EnemyHealth>().EnemyLives = 1;
                collision.gameObject.GetComponent<EnemyHealth>().HitEnemy();
            }
        }
    }

}
