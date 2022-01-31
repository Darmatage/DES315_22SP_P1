using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenThompson_DockingBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject playerExit;

    private static bool isPlayerInBoat = false;
    
    [SerializeField]
    GameObject dockedBoat;

    [SerializeField]
    SpriteRenderer prompt;

    [SerializeField]
    GameObject parkingIndicator;

    private bool playerInRange = false;
    private bool boatInRange = false;
    private GameObject boatCollidingWith = null;
    private bool justExitedBoat = false;

    private bool firstCollision = true;

    // Start is called before the first frame update
    void Start()
    {
        if(dockedBoat)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Show the prompt
        if (prompt && ((boatInRange && isPlayerInBoat && dockedBoat == null) || (playerInRange && !isPlayerInBoat && !justExitedBoat)))
        {
            prompt.enabled = true;
        }
        else
        {
            prompt.enabled = false;
        }

        if (boatInRange && isPlayerInBoat && boatCollidingWith)
        {
            // If the user presses E to exit the boat
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Set the boat tag back to normal
                boatCollidingWith.gameObject.tag = "BenThompsonBoat";

                // Get the player from the boat
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                // Get the boat's fixed joint component
                FixedJoint2D boatFixedJoint = boatCollidingWith.GetComponent<FixedJoint2D>();

                // If the fixed joint exists
                if (boatFixedJoint)
                {
                    // Detach the player as the connected body
                    boatFixedJoint.connectedBody = null;
                }

                // Get the boat's controller component
                BenThompson_BoatController boatController = boatCollidingWith.GetComponent<BenThompson_BoatController>();

                // If the boat controller exists
                if (boatController)
                {
                    // Set the status of the in boat flag to false because the player is no longer in the boat
                    boatController.SetPlayerInBoat(false);

                    AudioSource.PlayClipAtPoint(boatController.getInOutBoat, transform.position);

                    //GameObject controllerPlayer = boatController.GetPlayer();
                    //if(controllerPlayer)
                    //{
                    //    controllerPlayer.SetActive(false);
                    //}

                    boatController.GetComponent<BenThompson_BoatController>().HidePlayer();
                }

                // Unparent the player
                player.transform.parent = null;

                // Teleport the player to the player exit position
                player.transform.position = playerExit.transform.position;

                // Fix the player's rotation to be that of what it was prior to entering the boat
                if (boatController)
                {
                    player.transform.rotation = boatController.GetPlayerRotationBeforeEnteringBoat();
                }

                // Get the player's collider
                CircleCollider2D collider = player.GetComponent<CircleCollider2D>();

                // If the collider exists
                if (collider)
                {
                    // Enable the collider
                    collider.enabled = true;
                }

                // Get the player movement script
                PlayerMove pm = player.GetComponent<PlayerMove>();

                // If the player movement script exists
                if (pm)
                {
                    // Enable the player movement
                    pm.enabled = true;
                }

                // Get the player's rigidbody
                Rigidbody2D playerRigid = player.GetComponent<Rigidbody2D>();

                // If the player's rigidbody exists
                if (playerRigid)
                {
                    // Stop any motion remaining from a moving boat
                    playerRigid.velocity = Vector2.zero;
                }

                // Renable the player's sprite
                Transform playerArt = player.transform.Find("Player_art");
                if (playerArt)
                {
                    SpriteRenderer sp = playerArt.gameObject.GetComponent<SpriteRenderer>();
                    if (sp)
                    {
                        sp.enabled = true;
                    }
                }

                // Player is no longer in a boat
                isPlayerInBoat = false;

                // Set the docked boat to the boat the player has just left
                dockedBoat = boatCollidingWith.gameObject;

                // Eliminate the collision
                boatCollidingWith = null;

                // Just exited Boat
                justExitedBoat = true;
            }
        }
        else if (playerInRange && !isPlayerInBoat)
        {
            // If the user presses E to enter the boat
            if (Input.GetKeyDown(KeyCode.E) && dockedBoat)
            {
                BenThompson_BoatController boatController = dockedBoat.GetComponent<BenThompson_BoatController>();
                if (boatController)
                {
                    boatController.EnterBoat();
                    isPlayerInBoat = true;
                    dockedBoat.gameObject.tag = "Player";
                }
            }
        }

        // If the player is in the boat, enable the parking indicator child
        if(isPlayerInBoat)
        {
            parkingIndicator.SetActive(true);
        }
        // Otherwise have it disabled
        else
        {
            parkingIndicator.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the boat we are colliding with is docked and this is the first rounds of collision
        if (collision.gameObject.tag == "BenThompsonBoat" && firstCollision)
        {
            // Ignore it

            firstCollision = false;
            if (collision.gameObject == dockedBoat)
                return;
        }
        else if(dockedBoat == null)
        {
            firstCollision = false;
        }
            

        if (collision.gameObject.tag == "BenThompsonBoat" || (collision.gameObject.tag == "Player" && isPlayerInBoat))
        {
            boatCollidingWith = collision.gameObject;
            boatInRange = true;
        }
        else if (collision.gameObject.tag == "Player")
            playerInRange = true;
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    // Show the prompt
    //    if(prompt && ((collision.gameObject.tag == "BenThompsonBoat" && isPlayerInBoat && dockedBoat == null) || (collision.gameObject.tag == "Player" && !isPlayerInBoat)))
    //    {
    //        prompt.enabled = true;
    //    }

    //    if(collision.gameObject.tag == "BenThompsonBoat" && isPlayerInBoat)
    //    {
    //        // If the user presses E to exit the boat
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            // Get the player from the boat
    //            GameObject player = GameObject.FindGameObjectWithTag("Player");

    //            // Get the boat's fixed joint component
    //            FixedJoint2D boatFixedJoint = collision.gameObject.GetComponent<FixedJoint2D>();

    //            // If the fixed joint exists
    //            if (boatFixedJoint)
    //            {
    //                // Detach the player as the connected body
    //                boatFixedJoint.connectedBody = null;
    //            }

    //            // Get the boat's controller component
    //            BenThompson_BoatController boatController = collision.GetComponent<BenThompson_BoatController>();

    //            // If the boat controller exists
    //            if (boatController)
    //            {
    //                // Set the status of the in boat flag to false because the player is no longer in the boat
    //                boatController.SetPlayerInBoat(false);
    //            }

    //            // Unparent the player
    //            player.transform.parent = null;

    //            // Teleport the player to the player exit position
    //            player.transform.position = playerExit.transform.position;

    //            // Fix the player's rotation to be that of what it was prior to entering the boat
    //            if (boatController)
    //            {
    //                player.transform.rotation = boatController.GetPlayerRotationBeforeEnteringBoat();
    //            }

    //            // Get the player's collider
    //            CircleCollider2D collider = player.GetComponent<CircleCollider2D>();

    //            // If the collider exists
    //            if (collider)
    //            {
    //                // Enable the collider
    //                collider.enabled = true;
    //            }

    //            // Get the player movement script
    //            PlayerMove pm = player.GetComponent<PlayerMove>();

    //            // If the player movement script exists
    //            if (pm)
    //            {
    //                // Enable the player movement
    //                pm.enabled = true;
    //            }

    //            // Get the player's rigidbody
    //            Rigidbody2D playerRigid = player.GetComponent<Rigidbody2D>();

    //            // If the player's rigidbody exists
    //            if (playerRigid)
    //            {
    //                // Stop any motion remaining from a moving boat
    //                playerRigid.velocity = Vector2.zero;
    //            }

    //            // Renable the player's sprite
    //            Transform playerArt = player.transform.Find("Player_art");
    //            if (playerArt)
    //            {
    //                SpriteRenderer sp = playerArt.gameObject.GetComponent<SpriteRenderer>();
    //                if (sp)
    //                {
    //                    sp.enabled = true;
    //                }
    //            }

    //            // Player is no longer in a boat
    //            isPlayerInBoat = false;

    //            // Set the docked boat to the boat the player has just left
    //            dockedBoat = collision.gameObject;
    //        }
    //    }
    //    else if(collision.gameObject.tag == "Player" && !isPlayerInBoat)
    //    {
    //        // If the user presses E to enter the boat
    //        if (Input.GetKeyDown(KeyCode.E) && dockedBoat)
    //        {
    //            BenThompson_BoatController boatController = dockedBoat.GetComponent<BenThompson_BoatController>();
    //            if (boatController)
    //            {
    //                boatController.EnterBoat();
    //                isPlayerInBoat = true;
    //            }
    //        }
    //    }
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Disable the prompt
        if(prompt)
        {
            prompt.enabled = false;
        }

        // If the object that leaves is a boat
        if((collision.gameObject.tag == "BenThompsonBoat" && isPlayerInBoat) || (collision.gameObject.tag == "Player" && isPlayerInBoat))
        {
            // The boat that is docked is no longer docked
            dockedBoat = null;
        }

        if (collision.gameObject.tag == "BenThompsonBoat" || (collision.gameObject.tag == "Player" && isPlayerInBoat))
        {
            boatInRange = false;
            boatCollidingWith = null;
        }
        else if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            justExitedBoat = false;
        }
    }
}
