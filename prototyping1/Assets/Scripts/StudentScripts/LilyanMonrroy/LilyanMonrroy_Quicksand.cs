using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyanMonrroy_Quicksand : MonoBehaviour
{
    //Game Handler that manages the health decrease of the player
    private GameHandler gameHandlerObj;

    //Player reference
    private GameObject player;
    private Rigidbody2D playerRigidbody;
    private Transform playerTransform;
    private float playerMaxSpeed;

    //Quicksand variables.
    public float slowDownFactor = .5f; 
    public float moveSpeed = 1.0f;
    public int damage = 1;
    private bool inQuicksand;


    // Start is called before the first frame update
    void Start()
    {
        inQuicksand = false;
        player = null;

        //Initialize Game Handler so we can use it to decrease player health.
        if (GameObject.FindGameObjectWithTag("GameHandler") != null)
        {
            gameHandlerObj = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        }

        //Initializing the player with the scenes player game object.
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("LilyanMonrroy_Quicksand Log: Found the player!");

            playerRigidbody = player.GetComponent<Rigidbody2D>();
            playerTransform = player.GetComponent<Transform>();
            playerMaxSpeed = player.GetComponent<PlayerMove>().speed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("LilyanMonrroy_Quicksand Log: Trigger start!");
        
        if (other.gameObject.tag == "Player")
        {
            inQuicksand = true;
            //oldPlayerPos = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y, playerTransform.localPosition.z);
        }
        else
        {
            Destroy(other.gameObject);
            Debug.Log("LilyanMonrroy_Quicksand Log: Destroyed object that wasnt the player");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        inQuicksand = false;

        //Give player back normal movement speed before entering quicksand.
        player.GetComponent<PlayerMove>().speed = playerMaxSpeed; 
        Debug.Log("LilyanMonrroy_Quicksand Log: Trigger end!");
    }

    void FixedUpdate()
    {
        if (inQuicksand)
        {
            //slow down any speed the player is currently until speed is at 0.
            player.GetComponent<PlayerMove>().speed = Mathf.Lerp(0, player.GetComponent<PlayerMove>().speed, slowDownFactor);

            if (player.GetComponent<PlayerMove>().speed <= 0.1f)
            {
                player.GetComponent<PlayerMove>().speed = 0.0f;
            }

            if (Input.anyKey)
            {
                gameHandlerObj.TakeDamage(damage);
            }

            // if buttons are down
            // then sink and decrease players health.
            //else 
            //move player
        }
    }
}
