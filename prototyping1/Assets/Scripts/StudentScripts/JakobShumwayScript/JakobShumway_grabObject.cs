using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Hey, you! You're finally awake. You were trying to cross the border, right?
 
    Using my script is pretty easy.
        - Give a RigidBody2D to the grabbable object (don't forget about friction either).
        - Add 'GRAB_' to the start of the object name.
 */

public class JakobShumway_grabObject : MonoBehaviour
{
    public enum direction
    {
        up,
        down,
        left,
        right
    };

        [Tooltip("Which keybind would you like?")]
    public KeyCode grabButton;

        // How far to throw objects
        [Tooltip("How far to throw objects.")]
    public float throwIntensity = 15;

        // Move player opposite to throw force. Set this to 0 to disable.
        [Tooltip("Move player opposite to throw force. Set this to 0 to disable.")]
    public float playerThrowbackMultiplier = .5f;

        // After throwing, set speed to 0 for this timer.
        [Tooltip("After throwing, set speed to 0 for this timer. Made to be used with playerThrowback. Set this to 0 to disable.")]
    public float afterFreezeDuration = .35f;

    [HideInInspector]
    public float afterFreezeTimer = 0f;

    [HideInInspector]
    public bool holding = false;

    [HideInInspector]
    public direction playerDirection = direction.right;

    [HideInInspector]
    public Transform playerTrans;

    [HideInInspector]
    public GameObject grabbedObject;

    private float prevSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GetComponent<Transform>();
        prevSpeed = gameObject.GetComponent<PlayerMove>().speed;
    }

    // Update is called once per frame
    void Update()
    {
            // Remove player speed after throwing, makes throw more impactful
        if (afterFreezeTimer > 0)
        {
            afterFreezeTimer -= Time.deltaTime;
            gameObject.GetComponent<PlayerMove>().speed = 0;
        }
        else
        {
            gameObject.GetComponent<PlayerMove>().speed = prevSpeed;
        }



            // Update direction
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            playerDirection = direction.up;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            playerDirection = direction.down;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            playerDirection = direction.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            playerDirection = direction.right;

        

        if (grabbedObject)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ||
                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                grabbedObject.transform.position = playerTrans.transform.position;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                grabbedObject.transform.position += new Vector3(0, 1, 0);
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                grabbedObject.transform.position += new Vector3(0, -1.5f, 0);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                grabbedObject.transform.position += new Vector3(-1, 0, 0);
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                grabbedObject.transform.position += new Vector3(1, 0, 0);

            if (grabbedObject.GetComponent<EnemyHealth>())
                grabbedObject.GetComponent<EnemyHealth>().isStunned = true;
        }

        if (Input.GetKeyDown(grabButton))
        {
            if (holding)
            {
                float throwX = 0;
                float throwY = 0;

                // check if moving or not
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    throwY = throwIntensity;
                }
                else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    throwY = -throwIntensity;
                }

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    throwX = -throwIntensity;
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    throwX = throwIntensity;
                }

                if (throwX != 0 && throwY != 0)
                {
                    throwX /= 2;
                    throwY /= 2;
                }

                if (throwX != 0 || throwY != 0)
                    afterFreezeTimer = afterFreezeDuration;

                    // Launch Object
                grabbedObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwX, throwY), ForceMode2D.Impulse);

                    // Launch player in opposite direction
                throwX *= playerThrowbackMultiplier;
                throwY *= playerThrowbackMultiplier;
                playerTrans.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-throwX, -throwY), ForceMode2D.Impulse);

                if (grabbedObject.GetComponent<EnemyHealth>())
                    grabbedObject.GetComponent<EnemyHealth>().isStunned = false;

                // Set object down
                grabbedObject = null;
                holding = false;
            }
            else
            {
                    // look for and grab thing
                var tryGrab = FindObjectsOfType<Rigidbody2D>();
                int grabLength = tryGrab.Length;
                int closest = -1;
                for (int i = 0; i < grabLength; i++)
                {
                        // ignore anything that isn't meant to be grabbed
                    if (tryGrab[i].gameObject.ToString().StartsWith("GRAB_") == false)
                        continue;

                        // ignore everything that is behind the player
                    if (playerDirection == direction.up)
                    {
                        if (tryGrab[i].gameObject.transform.position.y < playerTrans.position.y)
                            continue;
                    }
                    if (playerDirection == direction.down)
                    {
                        if (tryGrab[i].gameObject.transform.position.y > playerTrans.position.y)
                            continue;
                    }
                    if (playerDirection == direction.left)
                    {
                        if (tryGrab[i].gameObject.transform.position.x > playerTrans.position.x)
                            continue;
                    }
                    if (playerDirection == direction.right)
                    {
                        if (tryGrab[i].gameObject.transform.position.x < playerTrans.position.x)
                            continue;
                    }

                    // ignore objects farther than 1

                    if (Mathf.Abs(tryGrab[i].gameObject.transform.position.x - playerTrans.position.x) > 2)
                        continue;

                    if (Mathf.Abs(tryGrab[i].gameObject.transform.position.y - playerTrans.position.y) > 2)
                        continue;

                    if (closest == -1)
                    {
                        closest = i;
                        continue;
                    }
                    else
                    {
                        if (Mathf.Abs(tryGrab[i].gameObject.transform.position.x - playerTrans.position.x) + Mathf.Abs(tryGrab[i].gameObject.transform.position.y - playerTrans.position.y)
                            < Mathf.Abs(tryGrab[closest].gameObject.transform.position.x - playerTrans.position.x) + Mathf.Abs(tryGrab[closest].gameObject.transform.position.y - playerTrans.position.y))
                            closest = i;
                        continue;
                    }
                }

                // grab i
                if (closest != -1)
                {
                    grabbedObject = tryGrab[closest].gameObject;
                    holding = true;

                    grabbedObject.transform.position = playerTrans.position;
                }
                
            }
        }
        // End of GrabButton
    }
}
