using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallForEnemy : MonoBehaviour
{
    void Start()
    {
        Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
        
    }
}
