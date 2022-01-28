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
            
            // Destoy projectile if it hits a wall
            if(collision.gameObject.name == "TilemapWalls")
                Destroy(transform.root.gameObject);

            // Destroy the projectile and any hit enemy
            else if(collision.gameObject.layer == 6)
            {
                Destroy(transform.root.gameObject);
                Destroy(collision.gameObject);
            }

            // Destroy the tile for the breakable wall
            else if(collision.gameObject.CompareTag("BraedanNeversBreakable"))
            {   
                    // Destroy the projectile and wall
                Destroy(transform.root.gameObject);
                Destroy(collision.gameObject);
            }
        }   
        
    }
}
