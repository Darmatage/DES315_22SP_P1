using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
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
    private Vector3 currChange;

    //Quicksand variables.
    public float slowDownFactor = .5f; 
    public float moveSpeed = 1.0f;
    public int damage = 1;
    private bool inQuicksand;
    private GameObject myCanvas;
    private GameObject ChangeDirText;
    private GameObject SinkingText;
    private GameObject MovingText;
    private GameObject SinkingMask;
    private GameObject MovingDust;
    private GameObject SinkingDust;
    private float currMaskYPos = 0.0f;
    private float maxMaskYPos = 2.3f;
    public float maskSpeed = 0.0f;
    public GameObject enemyExplosion;
    private int chancesToChangeDir = 0;


    // Start is called before the first frame update
    void Start()
    {
        inQuicksand = false;
        player = null;
        chancesToChangeDir = 0;

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

            maxMaskYPos = 2.3f;
            SinkingMask = playerTransform.Find("SinkingMask").gameObject;
            SinkingMask.SetActive(false);

            MovingDust = playerTransform.Find("MovingDust").gameObject;
            MovingDust.SetActive(false);

            SinkingDust = playerTransform.Find("SinkingDust").gameObject;
            SinkingDust.SetActive(false);
        }

        //Initializing canvas from the one in the scene.
        if(GameObject.Find("GameHandlerCanvas") != null)
        {
            myCanvas = GameObject.Find("GameHandlerCanvas").transform.Find("Canvas").gameObject;

            if (myCanvas)
            {
                SinkingText = myCanvas.transform.Find("SinkingPopUp").gameObject;
                MovingText = myCanvas.transform.Find("MovingPopUp").gameObject;
                ChangeDirText = myCanvas.transform.Find("ImageChangeDir").gameObject;

                SinkingText.SetActive(false);
                MovingText.SetActive(false);
                ChangeDirText.SetActive(false);
            }
        }
    }

    //Trigger event start
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("LilyanMonrroy_Quicksand Log: Trigger start!");
        if (other.gameObject.tag == "Player")
        {
            inQuicksand = true;

            ChangeDirText.SetActive(true);
            SinkingMask.SetActive(true);
            currMaskYPos = maxMaskYPos;
            SinkingMask.transform.localPosition = new Vector3(0, 0-currMaskYPos, 0);

            chancesToChangeDir = 3;//Default number of chances.

        }
        else
        {
            //If it is not a tile component delete it.
            if (other.gameObject.GetComponent<Tilemap>() == null)
            {
                //Instantiate explosion effect
                GameObject explosition = Instantiate(enemyExplosion, other.gameObject.transform.position, Quaternion.identity);
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
            //Deactivating UI for quicksand
            SinkingText.SetActive(false);
            MovingText.SetActive(false);
            SinkingMask.SetActive(false);
            MovingDust.SetActive(false);
            SinkingDust.SetActive(false);
            ChangeDirText.SetActive(false);

            inQuicksand = false;
            chancesToChangeDir = 0;
            player.GetComponent<PlayerMove>().speed = playerMaxSpeed;
        }

        //Debug.Log("LilyanMonrroy_Quicksand Log: Trigger end!");
    }

    //Update loop
    void FixedUpdate()
    {
        currChange = Vector3.zero;
        currChange.x = Input.GetAxisRaw("Horizontal");
        currChange.y = Input.GetAxisRaw("Vertical");

        if(currChange != Vector3.zero && change != currChange)
        {
            change = new Vector3(currChange.x, currChange.y, 0);
            if (inQuicksand)
            {
                --chancesToChangeDir;
            }
        }

        if (inQuicksand)
        {
            //slow down any speed the player is currently until speed is at 0.
            player.GetComponent<PlayerMove>().speed = Mathf.Lerp(0, player.GetComponent<PlayerMove>().speed, slowDownFactor);

            if (player.GetComponent<PlayerMove>().speed <= 0.1f)
            {
                player.GetComponent<PlayerMove>().speed = 0.0f;
            }

            //If there is any input, scale the player down and do damage.
            if (Input.anyKey && player.GetComponent<PlayerMove>().speed == 0)
            {
                if(chancesToChangeDir < 0)
                {
                    //Taking Damage from player
                    gameHandlerObj.TakeDamage(damage);
                    SinkingDust.SetActive(true);

                    //Updating position of mask
                    currMaskYPos = Mathf.Lerp(0, currMaskYPos, maskSpeed);
                    SinkingMask.transform.localPosition = new Vector3(0, 0 - currMaskYPos, 0);

                    if (GameHandler.PlayerHealth <= 0)
                    {
                        SinkingDust.SetActive(false);
                        SinkingMask.SetActive(false);
                    }

                }

                //switching UI text display.
                MovingDust.SetActive(false);

                /*MovingText.SetActive(false);
                SinkingText.SetActive(true);*/
            }
            else
            {
                if(player.GetComponent<PlayerMove>().speed == 0)
                {
                    SinkingDust.SetActive(false);
                    UpdateParticleDust();
                    /*SinkingText.SetActive(false);
                    MovingText.SetActive(true);*/

                    //not moving then move the player out of the sand.
                    playerRigidbody.MovePosition(playerTransform.position + change * moveSpeed * Time.deltaTime);
                }
            }
        }

        UpdateUIPositions();
    }

    void UpdateUIPositions()
    {
        if(chancesToChangeDir <= 0)
        {
            ChangeDirText.transform.Find("Text").gameObject.GetComponent<Text>().text = "Chances left to change direction: 0";
        }
        else
        {
            ChangeDirText.transform.Find("Text").gameObject.GetComponent<Text>().text = "Chances left to change direction: " + chancesToChangeDir;
        }

        SinkingText.transform.localPosition = new Vector3(player.transform.position.x, player.transform.position.y + 100.0f, player.transform.position.z);
        MovingText.transform.localPosition = new Vector3(player.transform.position.x, player.transform.position.y + 100.0f, player.transform.position.z);
    }

    void UpdateParticleDust()
    {
        MovingDust.SetActive(true);

        MovingDust.transform.localPosition = new Vector3(0, 0 , 0);
        MovingDust.transform.rotation = Quaternion.identity;

        if (change.x > 0)
        {
            MovingDust.transform.localPosition = new Vector3(0 + .85f, 0 , 0);
            MovingDust.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (change.x < 0)
        {
            MovingDust.transform.localPosition = new Vector3(0 + .85f, 0, 0);
            MovingDust.transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        if (change.y > 0)
        {
            MovingDust.transform.localPosition = new Vector3(0, 0 + 1.25f, 0);
            MovingDust.transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        if (change.y < 0)
        {
            MovingDust.transform.localPosition = new Vector3(0, 0 - 1.25f, 0);
            MovingDust.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }

}
