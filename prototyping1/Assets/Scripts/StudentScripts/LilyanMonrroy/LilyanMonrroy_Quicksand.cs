using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
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
    private float playerMaxYScale;
    private Vector3 change;

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
            playerMaxYScale = playerTransform.localScale.y;
        }
    }

    //Trigger event start
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("LilyanMonrroy_Quicksand Log: Trigger start!");

        if (other.gameObject.tag == "Player")
        {
            inQuicksand = true;

            //Get move direction that player was first going to.
            change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            //If it is not a tile component delete it.
            if (other.gameObject.GetComponent<Tilemap>() == null)
            {
                Destroy(other.gameObject);
                Debug.Log("LilyanMonrroy_Quicksand Log: Destroyed object.");
            }
        }

    }

    //Trigger event exit
    void OnTriggerExit2D(Collider2D other)
    {
        //Give player back normal movement speed before entering quicksand.
        if(other.gameObject.tag == "Player")
        {
            inQuicksand = false;

            player.GetComponent<PlayerMove>().speed = playerMaxSpeed;
            playerTransform.localScale = new Vector3(playerTransform.localScale.x, playerMaxYScale, playerTransform.localScale.z);
            playerRigidbody.MovePosition(playerTransform.position + change * moveSpeed * 20 * Time.deltaTime);
        }

        //Debug.Log("LilyanMonrroy_Quicksand Log: Trigger end!");
    }

    //Update loop
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

            //If there is any input, scale the player down and do damage.
            if (Input.anyKey)
            {
                gameHandlerObj.TakeDamage(damage);
                float newScale = Mathf.Lerp(0, playerTransform.localScale.y, .95f);
                playerTransform.localScale = new Vector3(playerTransform.localScale.x, newScale, playerTransform.localScale.z);
            }
            else
            {
                //not moving then move the player out of the sand.
                playerRigidbody.MovePosition(playerTransform.position + change *moveSpeed * Time.deltaTime);
            }
        }
    }
}
