using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Peeking in my scripts?

    This is just a simple col function.
 */

public class JakobShumway_impactDamage : MonoBehaviour
{
    public float forceRequired = 12;
    public int dmgAmount = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<EnemyHealth>())
        {
            if (Mathf.Abs(col.gameObject.GetComponent<Rigidbody2D>().velocity.x) + 
                Mathf.Abs(col.gameObject.GetComponent<Rigidbody2D>().velocity.y) > forceRequired)
            {
                for (int i = 0; i < dmgAmount; i++)
                    col.gameObject.GetComponent<EnemyHealth>().HitEnemy();
            }
        }
    }
}
