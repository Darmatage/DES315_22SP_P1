using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B18_Treadmill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherRB = collision.gameObject.GetComponent<Rigidbody2D>();

        if(otherRB != null)
        {
            otherRB.velocity += (Vector2)gameObject.transform.up * 3f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var otherRB = collision.gameObject.GetComponent<Rigidbody2D>();

        if (otherRB != null)
        {
            otherRB.velocity -= (Vector2)gameObject.transform.up * 3f;
        }
    }
}
