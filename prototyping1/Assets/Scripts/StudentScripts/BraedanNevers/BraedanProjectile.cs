using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BraedanProjectile : MonoBehaviour
{
    public bool isHeld = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHeld)
        {
            if(collision.gameObject.name == "TilemapWalls")
                Destroy(transform.root.gameObject);
            else if(collision.gameObject.layer == 6)
            {
                Destroy(transform.root.gameObject);
                Destroy(collision.gameObject);
            }
            else if(collision.gameObject.tag == "BraedanNeversBreakable")
            {
                Destroy(transform.root.gameObject);
                Destroy(collision.gameObject);
            }
        }
        
    }
}
