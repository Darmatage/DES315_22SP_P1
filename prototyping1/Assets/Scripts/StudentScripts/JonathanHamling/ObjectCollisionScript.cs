using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollisionScript : MonoBehaviour
{
    public bool wallCollision = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Walls")
        {
            wallCollision = true;
        }
        else
        {
            wallCollision = false;
        }
    }
}
