using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // UI control text
    [SerializeField]
    GameObject UIBoatControls;

    // Reference to the instantiated version of the ui text.
    GameObject UIControlsInstantiated;

    // Reference to the different children of the UI controls
    private GameObject forwardInfo;
    private GameObject rotateInfo;
    private GameObject stopInfo;

    // Timers to destroy the UI objects
    [SerializeField]
    public float UIDestructionWaitTime = 2.0f;

    // Flag indicating that this is the first time that we have entered a boat in the current scene.
    // Same for all boats across all scenes, so must be reset in update for different scenes.
    static private bool firstTimeRidingBoatInScene = true;

    // Boolean indicating whether the person has performed their first boat release
    private bool firstBoatRelease = true;

    // UI element for displaying how charged the boat is
    private GameObject boatChargeIndicator;

    [SerializeField]
    Color Green;

    [SerializeField]
    Color Yellow;

    [SerializeField]
    Color Orange;

    [SerializeField]
    Color Red;

    [SerializeField]
    GameObject boatFumes;

    [SerializeField]
    ParticleSystem boatWaterTrail;
    // Start is called before the first frame update
    void Start()
    {
        // Deactivate the player sprite
        playerSprite.SetActive(false);

        // Grab the boat charge indicator
        boatChargeIndicator = GameObject.Find("BenThompsonBoatChargeIndicator");
        if(boatChargeIndicator)
        {
            boatChargeIndicator.transform.parent.parent.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is in the boat
        if(playerInBoat)
        {
            // Keeps the player upright in the boat
            playerSprite.transform.up = Vector3.up;

            // Rotate the boat if necessary
            CheckRotate();

            // Move the boat forward
            CheckDrive();

            if(boatFumes)
            {
                boatFumes.SetActive(true);
            }
        }
        else
        {
            if (boatFumes)
            {
                boatFumes.SetActive(false);
            }

            if(boatWaterTrail)
            {
                boatWaterTrail.Stop();
            }
        }
    }

    private void CheckRotate()
    {
        if (boatBody.velocity.magnitude >= 0.25f)
        {
            boatWaterTrail.Play();
            return;
        }

        boatWaterTrail.Stop();

            

        // If the user holds the right arrow key or the D key ----> Rotating to the right
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            // Create a new rotation quaterion
            Quaternion newRotation = new Quaternion();

            // Rotate from the current rotation to the right
            newRotation.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z -  rotationChange);
            
            // Apply the new rotation
            transform.rotation = newRotation;

            // If the rotate info exists
            if(rotateInfo)
            {
                // The player should have understood the controls by now and we can remove them
                Destroy(rotateInfo, UIDestructionWaitTime);
            }
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

            // If the rotation info exists
            if (UIControlsInstantiated)
            {
                // The player should have understood the controls by now and we can remove them
                Destroy(rotateInfo, UIDestructionWaitTime);
            }
        }
    }

    private void CheckDrive()
    {
        // If the user holds the W button, make the boat go forwards
        if(Input.GetKey(KeyCode.W) && cooldownTimer <= 0.0f && boatBody.velocity.magnitude <= 0.25f)
        {
            // Increase the thrust of the boat
            boatThrust += 0.5f;

            // If the boat reaches a maximum thrust
            if(boatThrust > maxThrust)
            {
                // Cap the thrust
                boatThrust = maxThrust;
            }

            if(boatChargeIndicator)
            {
                // Enable the boat charge indicator
                boatChargeIndicator.transform.parent.parent.gameObject.SetActive(true);

                // Calculate the position where the charge indicator should be placed
                Vector3 chargePosition = transform.position + (2 * Vector3.right);

                // Set the boatChargeIndicator's position
                boatChargeIndicator.transform.parent.parent.position = Camera.main.WorldToScreenPoint(chargePosition);

                // Calculate the fill amount
                float chargeFill = boatThrust / maxThrust;

                // Get the Image component of the boat Charge indicator
                Image chargeImage = boatChargeIndicator.GetComponent<Image>();

                // If the image exists
                if(chargeImage)
                {
                    // Fill the image
                    chargeImage.fillAmount = chargeFill;

                    // Color the image depending on the interval
                    if(chargeFill <= 0.33f) // Green
                    {
                        chargeImage.color = Color.Lerp(Green, Yellow, chargeFill / 0.33f);
                    }
                    else if(chargeFill <= 0.66f) // Yellow
                    {
                        chargeImage.color = Color.Lerp(Yellow, Orange, (chargeFill - 0.33f) / 0.33f);
                    }
                    else // Red
                    {
                        chargeImage.color = Color.Lerp(Orange, Red, (chargeFill - 0.66f) / 0.33f);
                    }
                }
                
            } 
        }

        // When the user releases W to move
        else if(Input.GetKeyUp(KeyCode.W) && cooldownTimer <= 0.0f && boatBody.velocity.magnitude <= 0.25f)
        {
            if(boatChargeIndicator)
            {
                // Disable the boat charge indicator
                boatChargeIndicator.transform.parent.parent.gameObject.SetActive(false);
            }
           
            // Move the boat forwards
            boatBody.AddForce(transform.up * boatThrust, ForceMode2D.Impulse);

            // Apply the cooldown timer
            cooldownTimer = cooldownTime;

            // Reset the boat thrust
            boatThrust = minThrust;

            // If the forward movement info exists
            if (forwardInfo)
            {
                // The player should have understood the controls by now and we can remove them
                Destroy(forwardInfo, UIDestructionWaitTime);
            }

            // If this is not our first release
            if (!firstBoatRelease)
            {
                // If the stop info exists
                if(stopInfo)
                {
                    Destroy(stopInfo, UIDestructionWaitTime);
                }
            }
            else
            {
                // No longer our first boat release
                firstBoatRelease = false;
            }
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

           // Show the player as though they were in the boat
           playerSprite.SetActive(true);

           // If we are riding a boat for the first time in this scene
           if(firstTimeRidingBoatInScene)
           {
                // Display boat controls in the UI
                UIControlsInstantiated = Instantiate(UIBoatControls, GameObject.Find("Canvas").transform);

                forwardInfo = UIControlsInstantiated.transform.Find("MoveForward").gameObject;
                rotateInfo = UIControlsInstantiated.transform.Find("Rotate").gameObject;
                stopInfo = UIControlsInstantiated.transform.Find("StopInfo").gameObject;

                // It is no longer our first time riding a boat
                firstTimeRidingBoatInScene = false;
           }
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

    // Changes the UI Boat Control flag when changing scenes
    public static void GoingToNewScene()
    {
        // When we change scenes it will be our first time riding a boat again so we should
        // show the UI.
        firstTimeRidingBoatInScene = true;
    }

    private void ManageUIDeletionTimers()
    {

    }
}
