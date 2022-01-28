using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_AngledPipe : MonoBehaviour
{
    [SerializeField]
    private KeyCode rotateKey;

    //reference to player object
    private GameObject player;

    //how close we have to be to the pipe
    [SerializeField]
    private float proximity;

    private bool rotating;

    private float rotateTimer;
    [SerializeField]
    private float maxRotateTime;

    private Vector3 originalRot;
    private Vector3 newRot;

    [SerializeField]
    private GameObject pipeParticles;

    //public enum Orientations
    //{
    //    eRIGHT_DOWN,
    //    eLEFT_DOWN,
    //    eRIGHT_UP,
    //    eLEFT_UP
    //}
    //public Orientations orientation;

    // Start is called before the first frame update
    void Start()
    {
        //get the player object
        player = GameObject.FindGameObjectWithTag("Player");

        //DetermineOrientation();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        //in reality, playerPosition is the center of the player collider (which is near the legs)
        Vector3 playerPosition = new Vector3(player.transform.position.x - 0.15f, player.transform.position.y - 0.55f, 0);

        //only rotate when the player is in range of the cannon and it isn't already being pushed or pulled or rotated
        if (Vector3.Magnitude(transform.position - playerPosition) < proximity && !rotating)
        {
            //if we pressed this key while we were not already rotating
            if (Input.GetKey(KeyCode.Q))
            {
                //we are rotating
                rotating = true;
                rotateTimer = 0.0f;

                //get the original rotation of the cannon
                originalRot = transform.rotation.eulerAngles;
                //new rotation will be 90 degree counterclockwise
                newRot = originalRot + new Vector3(0, 0, 90.0f);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                //we are rotating
                rotating = true;
                rotateTimer = 0.0f;

                //get the original rotation of the cannon
                originalRot = transform.rotation.eulerAngles;
                //new rotation will be 90 degree clockwise
                newRot = originalRot - new Vector3(0, 0, 90.0f);
            }
        }

        //while we rotate
        if (rotating)
        {
            //lerp the rotation from the original orientation to the new one
            transform.eulerAngles = Vector3.Lerp(originalRot, newRot, rotateTimer / maxRotateTime);
            //increment rotation timer
            rotateTimer += Time.deltaTime;
        }

        //if the z rotation ever surpasses 360 degrees, subtract 360 from it to essentially revert it
        if (transform.eulerAngles.z >= 360.0f)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 360.0f);
        }

        //if the rotation has finished
        if (rotateTimer >= maxRotateTime)
        {
            //stop the timer
            rotateTimer = maxRotateTime;
            //we are no longer rotating
            rotating = false;

            //there is a slight bit of error with lerping rotations, so to ensure the angle is perfect, assign it
            transform.eulerAngles = newRot;

            //DetermineOrientation();
        }

    }

    //private void DetermineOrientation()
    //{
    //    //seeing how the pipe is oriented by comparing transform vectors with global vectors (i.e. tranform.right vs. Vector3.right)

    //    //  |__
    //    if (Vector3.up == transform.up && Vector3.right == transform.right)
    //    {
    //        orientation = Orientations.eRIGHT_UP;
    //    }
    //    //   __|
    //    else if (Vector3.up == transform.right && -Vector3.right == transform.up)
    //    {
    //        orientation = Orientations.eLEFT_UP;
    //    }
    //    //   __
    //    //  |
    //    else if (-Vector3.up == transform.right && Vector3.right == transform.up)
    //    {
    //        orientation = Orientations.eRIGHT_DOWN;
    //    }
    //    //  __
    //    //    |
    //    else if (-Vector3.up == transform.up && -Vector3.right == transform.right)
    //    {
    //        orientation = Orientations.eLEFT_DOWN;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if a cannonball hit the pipe
        if (collision.gameObject.name.Contains("Nunes_Cannonball"))
        {
            if (collision.transform.right == -transform.up)
            {
                //center the object on the pipe
                collision.transform.position = transform.position;
                //rotate +90 degrees
                collision.transform.eulerAngles = new Vector3(0, 0, collision.transform.eulerAngles.z + 90.0f);
                //set the velocity with the newly-rotated right vector
                collision.GetComponent<Rigidbody2D>().velocity = collision.transform.right * collision.GetComponent<DanielNunes_Cannonball>().GetSpeed();

                //create pipe particles
                GameObject p = Instantiate(pipeParticles, null);
                p.transform.position = transform.position;
                //match the angle of the particles with the cannonball (when cannonball is at z = 0, particles are at z = -90)
                //match the angles exactly initially...
                p.transform.eulerAngles = collision.transform.eulerAngles;
                //..then apply the 90-degree offset
                p.transform.eulerAngles = new Vector3(0, 0, collision.transform.eulerAngles.z - 90.0f);
            }
            else if (collision.transform.right == -transform.right)
            {
                //center the object on the pipe
                collision.transform.position = transform.position;
                //rotate -90 degrees
                collision.transform.eulerAngles = new Vector3(0, 0, collision.transform.eulerAngles.z - 90.0f);
                //set the velocity with the newly-rotated right vector
                collision.GetComponent<Rigidbody2D>().velocity = collision.transform.right * collision.GetComponent<DanielNunes_Cannonball>().GetSpeed();

                //create pipe particles
                GameObject p = Instantiate(pipeParticles, null);
                p.transform.position = transform.position;
                //match the angle of the particles with the cannonball (when cannonball is at z = 0, particles are at z = -90)
                //match the angles exactly initially...
                p.transform.eulerAngles = collision.transform.eulerAngles;
                //..then apply the 90-degree offset
                p.transform.eulerAngles = new Vector3(0, 0, collision.transform.eulerAngles.z - 90.0f);
            }
            else
            {
                collision.gameObject.GetComponent<DanielNunes_Cannonball>().CreateParticles();
                //destroy the canonball if it didn't enter either end of the pipe
                Destroy(collision.gameObject);
            }
        }
    }
}
