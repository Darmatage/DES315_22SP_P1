using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPusher : MonoBehaviour
{
    public float Force = 3f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 pushDir = (collision.transform.position - transform.parent.position).normalized;
            collision.transform.position += pushDir * Force * Time.deltaTime;
        }
    }
}
