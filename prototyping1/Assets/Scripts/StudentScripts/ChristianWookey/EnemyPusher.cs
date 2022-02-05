using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPusher : MonoBehaviour
{
    public float Force = 3f;
    public bool StunEnemies = false;
    public float StunTime = 2f;
    public bool LinearPush = false;

    public Vector3 LinearPushDir;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 pushDir;
            if (LinearPush)
            {
                pushDir = LinearPushDir;
                collision.GetComponent<Rigidbody2D>().MovePosition(collision.transform.position + pushDir * Force * Time.fixedDeltaTime);
            }
            else
            {
                pushDir = (collision.transform.position - transform.parent.position).normalized;
                collision.transform.position += pushDir * Force * Time.deltaTime;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (StunEnemies)
            {
                collision.gameObject.GetComponent<EnemyHealth>().stunTime = StunTime;
                collision.gameObject.GetComponent<EnemyHealth>().isStunned = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (StunEnemies)
            {
                collision.gameObject.GetComponent<EnemyHealth>().stunTime = StunTime;
                collision.gameObject.GetComponent<EnemyHealth>().isStunned = true;
            }
        }
    }
}
