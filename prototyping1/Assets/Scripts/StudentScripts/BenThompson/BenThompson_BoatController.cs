using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenThompson_BoatController : MonoBehaviour
{
    public GameObject boatSprite;
    public GameObject playerSprite;

    // How much the angle should change when rotating the boat
    [SerializeField]
    float rotationChange = 0.35f;

    // The rigid body of the boat
    [SerializeField]
    Rigidbody2D boatBody;

    // How fast should the boat move in one interval
    private float boatThrust = 1.0f;

    // Joint so that the player can be parented to the boat since both have rigidbodies.
    [SerializeField]
    FixedJoint2D fixedJointPlayer;

    // Timer to stop the player from moving continously with the boat, only in bursts.
    private float cooldownTimer = 0.0f;

    [SerializeField]
    float cooldownTime = 2.0f;

    [SerializeField]
    float maxThrust = 100.0f;

    [SerializeField]
    float minThrust = 25.0f;

    // Is the player in the boat at this momement?
    private bool playerInBoat = false;

    private Quaternion playerRotationBeforeBoat = new Quaternion();

    // Start is called before the first frame update
    void Start()
    {
        playerSprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is in the boat
        if(playerInBoat)
        {
            // Rotate the boat if necessary
            CheckRotate();

            // Move the boat forward
            CheckDrive();
        }
    }

    private void CheckRotate()
    {
        if (boatBody.velocity.magnitude >= 0.25f)
            return;

        // If the user holds the right arrow key or the D key ----> Rotating to the right
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            // Create a new rotation quaterion
            Quaternion newRotation = new Quaternion();

            // Rotate from the current rotation to the right
            newRotation.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z -  rotationChange);
            
            // Apply the new rotation
            transform.rotation = newRotation;
        }

        // If the user holds the left arrow key or the A key ----> Rotating to the left
        else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            // Create a new rotation quaterion
            Quaternion newRotation = new Quaternion();

            // Rotate from the current rotation to the left
            newRotation.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + rotationChange);

            // Apply the new rotation
            transform.rotation = newRotation;
        }
    }

    private void CheckDrive()
    {
        // If the user holds the W button, make the boat go forwards
        if(Input.GetKey(KeyCode.W) && cooldownTimer <= 0.0f && boatBody.velocity.magnitude <= 0.25f)
        {
            // Increase the thrust of the boat
            boatThrust += 1;

            // If the boat reaches a maximum thrust
            if(boatThrust > maxThrust)
            {
                // Cap the thrust
                boatThrust = maxThrust;
            }
        }

        // When the user releases W to move
        else if(Input.GetKeyUp(KeyCode.W) && cooldownTimer <= 0.0f && boatBody.velocity.magnitude <= 0.25f)
        {
            // Move the boat forwards
            boatBody.AddForce(transform.up * boatThrust, ForceMode2D.Impulse);

            // Apply the cooldown timer
            cooldownTimer = cooldownTime;

            // Reset the boat thrust
            boatThrust = minThrust;
        }

        // If the cooldown timer isn't done
        else if (cooldownTimer >= 0.0f)
        {
            // Decrease the amount of time remaining
            cooldownTimer -= Time.deltaTime;

            // If the timer is done
            if(cooldownTimer <= 0.0f)
            {
                // Set the timer to be 0
                cooldownTimer = 0.0f;
            }
        }
    }

    public void EnterBoat()
    {
       GameObject player = GameObject.FindGameObjectWithTag("Player");
       if(player)
       {
           // Get the player's original rotation
           playerRotationBeforeBoat = player.transform.rotation;

           CircleCollider2D collider = player.GetComponent<CircleCollider2D>();
           if(collider)
           {
               collider.enabled = false;
           }

           player.transform.parent = transform;
           player.transform.position = transform.position;

           PlayerMove pm = player.GetComponent<PlayerMove>();
           if(pm)
           {
               pm.enabled = false;
           }

           Rigidbody2D playerRigid = player.GetComponent<Rigidbody2D>();
           if(playerRigid)
           {
               fixedJointPlayer.connectedBody = playerRigid;
           }

           Transform playerArt = player.transform.Find("Player_art");
           if(playerArt)
           {
               SpriteRenderer sp = playerArt.gameObject.GetComponent<SpriteRenderer>();
               if(sp)
               {
                   sp.enabled = false;
               }
           }

           // Player is now in the boat
           playerInBoat = true;

           playerSprite.SetActive(true);
       }
    }

    public void SetPlayerInBoat(bool value)
    {
        playerInBoat = value;
    }

    public Quaternion GetPlayerRotationBeforeEnteringBoat()
    {
        return playerRotationBeforeBoat;
    }

    //public GameObject GetPlayer()
    //{
    //    return player;
    //}

    public void HidePlayer()
    {
        playerSprite.SetActive(false);
    }
}
