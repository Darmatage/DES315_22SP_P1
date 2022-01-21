using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraedanProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
        {
        if(collision.gameObject.name == "TilemapWalls")
            Destroy(transform.root.gameObject);
        else if(collision.gameObject.layer == 6)
        {
            Destroy(transform.root.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
