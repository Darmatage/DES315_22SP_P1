using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyJanis_FlyingSpikes : MonoBehaviour
{
    private GameHandler gameHandlerObj;
    public int damage = 10;

    Rigidbody2D rb;
    PolygonCollider2D polycollider;
    SpriteRenderer sprite;
    
    //The Color to be assigned to the Renderer’s Material
    public Color color;
    public Color flashColor;

    bool isFlying = false;

    Vector2 dir = Vector2.down;
    public enum Direction{Up,Right,Down, Left};

    public Direction flyDirection;
    public float launchDelay;
    public float speed = 200.0f;
    public float detectDistance;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        polycollider = GetComponent<PolygonCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        //sprite.color = color;

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
            RaycastHit2D detected = Physics2D.Raycast(transform.position, dir, detectDistance);

            Debug.DrawRay(transform.position, dir * detectDistance, Color.red);

            if(detected.transform != null)
            {
                if(detected.transform.tag == "Player")
                {
                    sprite.color = flashColor; //Color.Lerp(color,flashColor,Mathf.Sin(Time.time));

                    Invoke("LaunchSpike",launchDelay);
                    
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

    void LaunchSpike()
    {
        rb.AddForce(dir * speed);
    }

}
