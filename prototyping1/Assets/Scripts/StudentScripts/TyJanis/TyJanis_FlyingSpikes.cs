using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyJanis_FlyingSpikes : MonoBehaviour
{
    private GameHandler gameHandlerObj;
    public int damage = 10;

    Rigidbody2D rb;
    PolygonCollider2D collider;
    public float distance;
    bool isFlying = false;

    public float speed = 200.0f;
    Vector2 dir = Vector2.down;
    public enum Direction{Up,Right,Down, Left};
    public Direction flyDirection;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<PolygonCollider2D>();

        if(flyDirection == Direction.Up)
        {
            dir = Vector2.up;
        }
        else if(flyDirection == Direction.Right)
        {
            dir = Vector2.right;
        }
        else if(flyDirection == Direction.Down)
        {
            dir = Vector2.down;
        }
        else if(flyDirection == Direction.Left)
        {
            dir = Vector2.left;
        }

        GameObject gameHandlerLocation = GameObject.FindWithTag ("GameHandler");
		if (gameHandlerLocation != null) 
        {
			gameHandlerObj = gameHandlerLocation.GetComponent<GameHandler> ();
		}

    }

    private void Update()
    {
        Physics2D.queriesStartInColliders = false;

        if(!isFlying)
        {
            RaycastHit2D detected = Physics2D.Raycast(transform.position, dir, distance);

            Debug.DrawRay(transform.position, dir * distance, Color.red);

            if(detected.transform != null)
            {
                if(detected.transform.tag == "Player")
                {
                    rb.AddForce(dir * speed);
                    isFlying = true;
                }
            }
        }     
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameHandlerObj.TakeDamage(damage);
            Destroy (gameObject);
		}
		
	}
}
