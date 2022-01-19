using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_Cannon : MonoBehaviour
{
    [SerializeField]
    private GameObject cannonball;

    //if the cannon can be fired
    private bool usable;
    //if the player is holding onto the cannon
    private bool clutching;

    private bool pushing;

    private float moveTimer;
    [SerializeField]
    private float maxMoveTime;

    public enum Where
    {
        eRIGHT,
        eUP,
        eLEFT,
        eDOWN
    }

    public Where whereIsPlayer;

    [SerializeField]
    private float speed;

    private Vector2 f_right;
    private Vector2 f_up;
    private Vector2 f_left;
    private Vector2 f_down;

    // Start is called before the first frame update
    void Start()
    {
        moveTimer = maxMoveTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (usable && !pushing && Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(cannonball, transform);
        }

        //if we are pushing the cannon
        if (clutching)
        {
            //if pushing right
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W))
            {
                pushing = true;
            }
        }

        if (pushing)
        {
            //increment timer
            moveTimer += Time.deltaTime;

            //while the timer hasn't finished
            if (moveTimer < maxMoveTime)
            {
                switch (whereIsPlayer)
                {
                    //if player is on right, push left
                    case Where.eRIGHT:
                        transform.position = Vector2.MoveTowards(transform.position, f_left, moveTimer / maxMoveTime);
                        break;
                    //if player is above, push down
                    case Where.eUP:
                        transform.position = Vector2.MoveTowards(transform.position, f_down, moveTimer / maxMoveTime);
                        break;
                    //if player is on left, push right
                    case Where.eLEFT:
                        transform.position = Vector2.MoveTowards(transform.position, f_right, moveTimer / maxMoveTime);
                        break;
                    //if player is below, push up
                    case Where.eDOWN:
                        transform.position = Vector2.MoveTowards(transform.position, f_up, moveTimer / maxMoveTime);
                        break;
                }
            }
            //if timer has finished
            else
            {
                moveTimer = maxMoveTime;
                pushing = false;
            }
        }
    }

    private void GetPlayerDirection(GameObject player)
    {
        //get the size of the box collider of the child
        Vector2 collisionSize = transform.Find("Collider").GetComponent<BoxCollider2D>().size;

        Vector3 playerPosition = new Vector3(player.transform.position.x - 0.12f, player.transform.position.y - 0.62f, 0);
        //get the vector made by the player's position and the cannon
        Vector2 vec = transform.position - playerPosition;

        Debug.Log("PLAYER: " + playerPosition);
        Debug.Log("CANNON: " + transform.position);
        ////get the angle (in radians) between each of the 4 direction vectors: right, -right, up, -up
        //float theta;
        //float best = 2 * Mathf.PI;

        ////get the angle between vec and the right vector
        //theta = Mathf.Acos(Vector3.Dot(vec, Vector3.right) / (Vector3.Magnitude(vec) * Vector3.Magnitude(Vector3.right)));

        //if (Mathf.Abs(theta) < best)
        //{
        //    best = theta;
        //    whereIsPlayer = Where.eRIGHT;
        //}

        ////get the angle between vec and the left vector
        //theta = Mathf.Acos(Vector3.Dot(vec, -Vector3.right) / (Vector3.Magnitude(vec) * Vector3.Magnitude(-Vector3.right)));

        //if (Mathf.Abs(theta) < best)
        //{
        //    best = theta;
        //    whereIsPlayer = Where.eLEFT;
        //}

        ////get the angle between vec and the up vector
        //theta = Mathf.Acos(Vector3.Dot(vec, Vector3.up) / (Vector3.Magnitude(vec) * Vector3.Magnitude(Vector3.up)));

        //if (Mathf.Abs(theta) < best)
        //{
        //    best = theta;
        //    whereIsPlayer = Where.eUP;
        //}

        ////get the angle between vec and the down vector
        //theta = Mathf.Acos(Vector3.Dot(vec, -Vector3.up) / (Vector3.Magnitude(vec) * Vector3.Magnitude(-Vector3.up)));

        //if (Mathf.Abs(theta) < best)
        //{
        //    best = theta;
        //    whereIsPlayer = Where.eDOWN;
        //}

        //if on left
        if (playerPosition.x < transform.position.x && playerPosition.y < transform.position.y + collisionSize.y && playerPosition.y > transform.position.y - collisionSize.y)
        {
            whereIsPlayer = Where.eLEFT;
        }
        //if on right
        if (playerPosition.x > transform.position.x && playerPosition.y < transform.position.y + collisionSize.y && playerPosition.y > transform.position.y - collisionSize.y)
        {
            whereIsPlayer = Where.eRIGHT;
        }
        //if above
        if (playerPosition.y > transform.position.y && playerPosition.x < transform.position.x + collisionSize.x && playerPosition.x > transform.position.x - collisionSize.x)
        {
            whereIsPlayer = Where.eUP;
        }
        //if below
        if (playerPosition.y < transform.position.y && playerPosition.x < transform.position.x + collisionSize.x && playerPosition.x > transform.position.x - collisionSize.x)
        {
            whereIsPlayer = Where.eDOWN;
        }

        //pushing = true;

        /*
                  [f_up]
         [f_left][cannon][f_right]
                 [f_down]
        */
        //update the future positions based on the cannon's current position (by one unit)
        f_right = new Vector2(transform.position.x + 1.0f, transform.position.y);
        f_up    = new Vector2(transform.position.x, transform.position.y + 1.0f);
        f_left  = new Vector2(transform.position.x - 1.0f, transform.position.y);
        f_down  = new Vector2(transform.position.x, transform.position.y - 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            usable = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if we have stayed on the cannon
        if (collision.gameObject.CompareTag("Player"))
        {
            //see where the player is relative to the cannon
            GetPlayerDirection(collision.gameObject);

            //if we press this key
            if (Input.GetKeyDown(KeyCode.O) && moveTimer >= maxMoveTime)
            {
                //we are now clutching the cannon
                clutching = true;
                pushing = true;
                moveTimer = 0.0f;
            }
            else if (Input.GetKeyUp(KeyCode.O))
            {
                clutching = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            usable = false;
        }
    }
}
