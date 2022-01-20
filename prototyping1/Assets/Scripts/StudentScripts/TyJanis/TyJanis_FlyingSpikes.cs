using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyJanis_FlyingSpikes : MonoBehaviour
{
    Rigidbody2D rb;
    PolygonCollider2D collider;
    public float distance;
    bool isFlying = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<PolygonCollider2D>();

    }

    private void Update()
    {
        Physics2D.queriesStartInColliders = false;

        if(!isFlying)
        {
            RaycastHit2D detected = Physics2D.Raycast(transform.position, Vector2.down, distance);

            Debug.DrawRay(transform.position, Vector2.down * distance, Color.red);

            if(detected.transform != null)
            {
                if(detected.transform.tag == "Player")
                {
                    rb.gravityScale = 3;
                    isFlying = true;
                }
            }
        }     
    }
}
