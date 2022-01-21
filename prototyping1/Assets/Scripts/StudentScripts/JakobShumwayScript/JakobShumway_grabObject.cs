using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JakobShumway_grabObject : MonoBehaviour
{
    public enum direction
    {
        up,
        down,
        left,
        right
    };

    public KeyCode grabButton;

    [HideInInspector]
    public bool holding = false;

    public direction playerDirection = direction.right;

    [HideInInspector]
    public Transform playerTrans;

    [HideInInspector]
    public GameObject grabbedObject;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
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

        }

        if (Input.GetKeyDown(grabButton))
        {
            if (holding)
            {
                int throwX = 0;
                int throwY = 0;

                // check if moving or not
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    throwY = 15;
                }
                else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    throwY = -15;
                }

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    throwX = -15;
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    throwX = 15;
                }

                grabbedObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwX, throwY), ForceMode2D.Impulse);

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
                grabbedObject = tryGrab[closest].gameObject;
                holding = true;

                grabbedObject.transform.position = playerTrans.position;
            }
        }
        // End of GrabButton
    }
}
