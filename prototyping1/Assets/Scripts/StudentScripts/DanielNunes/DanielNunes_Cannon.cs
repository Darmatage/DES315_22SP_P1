using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_Cannon : MonoBehaviour
{
    [SerializeField]
    private KeyCode shootKey;
    [SerializeField]
    private KeyCode holdKey;
    [SerializeField]
    private KeyCode rotateKey;

    [SerializeField]
    private GameObject cannonball;

    //if the cannon can be fired
    public bool usable;
    //for manipulating controls panel
    //public bool inRange;
    //public bool heldOnto;

    private bool pushing;
    private bool pulling;
    private bool rotating;
    private bool sinking;

    //if there are particular tiles directly next to the cannon
    private bool somethingOnRight;
    private bool somethingOnLeft;
    private bool somethingOnTop;
    private bool somethingOnBottom;

    //if there are particular tiles two tiles away from the cannon
    private bool somethingNearRight;
    private bool somethingNearLeft;
    private bool somethingNearTop;
    private bool somethingNearBottom;

    //if a raycast picks up the player near the cannon
    private bool playerHere;

    private float playerSpeed;

    //where the player is relative to the cannon
    public enum Where
    {
        eRIGHT, //to the right of the cannon
        eUP,    //above the cannon
        eLEFT,  //to the left of the cannon
        eDOWN,  //below the cannon
        //eNOWHERE
    }

    public Where whereIsPlayer;
    private Where tempWhereIsPlayer;

    //timers
    private float shrinkTimer;
    private float rotateTimer;
    private float moveTimer;
    [SerializeField]
    private float maxMoveTime;

    //movement speed factor of the cannon when pushed or pulled
    [SerializeField]
    private float speed;

    //positions
    private Vector2 origin;
    private Vector2 f_right;
    private Vector2 f_up;
    private Vector2 f_left;
    private Vector2 f_down;

    //rotations
    private Vector3 originalRot;
    private Vector3 newRot;

    //reference to player object
    private GameObject player;
    private Vector2 playerLockPosition;

    //how close we have to be to the cannon
    [SerializeField]
    private float proximity;

    // Start is called before the first frame update
    void Start()
    {
        moveTimer = maxMoveTime;
        player = GameObject.FindGameObjectWithTag("Player");
        playerSpeed = player.GetComponent<PlayerMove>().speed;

        //round the position of the cannon to 1 decimal place so it's as precise as possible
        transform.position = new Vector2((float)Mathf.Round(transform.position.x * 10.0f) / 10.0f, (float)Mathf.Round(transform.position.y * 10.0f) / 10.0f);
        //approximate where the cannon should be if the user doesn't place it properly initially
        FixPositioning();
    }

    // Update is called once per frame
    void Update()
    {
        RangeAndInputCheck();
        
        //if the cannon is usable, isn't being pushed and/or pulled, and we press the shoot button
        //we also cannot shoot if there is already an instance of a cannonball
        if (usable && !pushing && !pulling && !rotating && Input.GetKeyDown(shootKey))
        {
            //fire a cannonball
            Instantiate(cannonball, transform);
        }

        ManagePushingAndPulling();

        //if the player is pushing or pulling the cannon
        if (pushing || pulling)
        {
            //lock their movement by constantly setting their velocity to 0
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            //next make the player a child of the cannon so it moves along with it
            player.transform.parent = transform;

            player.GetComponent<PlayerMove>().speed = 0.0f;
        }

        Rotate();
        
        if (rotating)
        {
            player.transform.position = playerLockPosition;
            //lock their movement by constantly setting their velocity to 0
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        //if the z rotation ever surpasses 360 degrees, subtract 360 from it to essentially revert it
        if (transform.eulerAngles.z >= 360.0f)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 360.0f);
        }

        Sink();

        if (!sinking)
        {
            OnTarosTiles();
        }
    }

    private void GetPlayerDirection(GameObject player)
    {
        //get the size of the box collider
        Vector2 collisionSize = GetComponent<BoxCollider2D>().size;

        Vector3 playerPosition = new Vector3(player.transform.position.x - 0.15f, player.transform.position.y - 0.55f, 0);

        //if on left
        if (playerPosition.x < transform.position.x && playerPosition.y < transform.position.y + collisionSize.y && playerPosition.y > transform.position.y - collisionSize.y)
        {
            whereIsPlayer = Where.eLEFT;
        }
        //if on right
        else if (playerPosition.x > transform.position.x && playerPosition.y < transform.position.y + collisionSize.y && playerPosition.y > transform.position.y - collisionSize.y)
        {
            whereIsPlayer = Where.eRIGHT;
        }
        //if above
        else if (playerPosition.y > transform.position.y && playerPosition.x < transform.position.x + collisionSize.x && playerPosition.x > transform.position.x - collisionSize.x)
        {
            whereIsPlayer = Where.eUP;
        }
        //if below
        else if (playerPosition.y < transform.position.y && playerPosition.x < transform.position.x + collisionSize.x && playerPosition.x > transform.position.x - collisionSize.x)
        {
            whereIsPlayer = Where.eDOWN;
        }
    }

    private void ManagePushingAndPulling()
    {
        if (pushing)
        {
            //while the timer hasn't finished
            if (moveTimer < maxMoveTime)
            {
                switch (tempWhereIsPlayer)
                {
                    //if player is on right, push left
                    case Where.eRIGHT:
                        transform.position = Vector3.MoveTowards(origin, f_left, moveTimer / maxMoveTime);
                        //increment timer
                        moveTimer += Time.deltaTime;
                        break;
                    //if player is above, push down
                    case Where.eUP:
                        transform.position = Vector3.MoveTowards(origin, f_down, moveTimer / maxMoveTime);
                        //increment timer
                        moveTimer += Time.deltaTime;
                        break;
                    //if player is on left, push right
                    case Where.eLEFT:
                        transform.position = Vector3.MoveTowards(origin, f_right, moveTimer / maxMoveTime);
                        //increment timer
                        moveTimer += Time.deltaTime;
                        break;
                    //if player is below, push up
                    case Where.eDOWN:
                        transform.position = Vector3.MoveTowards(origin, f_up, moveTimer / maxMoveTime);
                        //increment timer
                        moveTimer += Time.deltaTime;
                        break;
                }
            }
            //if timer has finished
            else
            {
                moveTimer = maxMoveTime;
                pushing = false;

                //round the position of the cannon to 1 decimal place so it's as precise as possible
                transform.position = new Vector2((float)Mathf.Round(transform.position.x * 10.0f) / 10.0f, (float)Mathf.Round(transform.position.y * 10.0f) / 10.0f);

                FixPositioning();

                //reset parenting
                player.transform.parent = null;

                ResetContacts();

                player.GetComponent<PlayerMove>().speed = playerSpeed;
            }
        }
        else if (pulling)
        {
            //while the timer hasn't finished
            if (moveTimer < maxMoveTime)
            {
                switch (tempWhereIsPlayer)
                {
                    //if player is on right, pull right
                    case Where.eRIGHT:
                        transform.position = Vector3.Lerp(origin, f_right, moveTimer / maxMoveTime);
                        //increment timer
                        moveTimer += Time.deltaTime;
                        break;
                    //if player is above, pull up
                    case Where.eUP:
                        transform.position = Vector3.Lerp(origin, f_up, moveTimer / maxMoveTime);
                        //increment timer
                        moveTimer += Time.deltaTime;
                        break;
                    //if player is on left, pull left
                    case Where.eLEFT:
                        transform.position = Vector3.Lerp(origin, f_left, moveTimer / maxMoveTime);
                        //increment timer
                        moveTimer += Time.deltaTime;
                        break;
                    //if player is below, pull down
                    case Where.eDOWN:
                        transform.position = Vector3.Lerp(origin, f_down, moveTimer / maxMoveTime);
                        //increment timer
                        moveTimer += Time.deltaTime;
                        break;
                }
            }
            //if timer has finished
            else
            {
                moveTimer = maxMoveTime;
                pulling = false;

                //round the position of the cannon to 1 decimal place so it's as precise as possible
                transform.position = new Vector2((float)Mathf.Round(transform.position.x * 10.0f) / 10.0f, (float)Mathf.Round(transform.position.y * 10.0f) / 10.0f);

                FixPositioning();

                //reset parenting
                player.transform.parent = null;

                ResetContacts();

                player.GetComponent<PlayerMove>().speed = playerSpeed;
            }
        }
    }

    private void RangeAndInputCheck()
    {
        RaycastCheck();

        //do not allow pushing or pulling if the cannon is in the middle of sinking and respawning or rotating
        if (sinking || transform.localScale != new Vector3(1,1,1) || rotating)
        {
            return;
        }

        //in reality, playerPosition is the center of the player collider (which is near the legs)
        Vector3 playerPosition = new Vector3(player.transform.position.x - 0.15f, player.transform.position.y - 0.55f, 0);

        //only push when the player is in range of the cannon and it isn't already being pushed or pulled
        if (Vector3.Magnitude(transform.position - playerPosition) < proximity && moveTimer >= maxMoveTime)
        {
            //see where the player is relative to the cannon
            GetPlayerDirection(player);

            //if (!heldOnto)
            //{
            //    inRange = true;
            //}

            //if we hold down this key, we are holding onto the cannon
            if (Input.GetKey(holdKey))
            {
                //heldOnto = true;
                //inRange = false;

                //snap the player depending on where they are located relative to the cannon
                SnapPlayer();

                //maybe have a change in animation to indicate holding the cannon

                //if we pressed left while to the cannon's left
                //if we pressed right while to the cannon's right
                //if we pressed up while above the cannon
                //if we pressed down while below the cannon
                //if any of these conditions were met, we are pulling the cannon
                if (Input.GetAxisRaw("Horizontal") < 0 && whereIsPlayer == Where.eLEFT ||
                    Input.GetAxisRaw("Horizontal") > 0 && whereIsPlayer == Where.eRIGHT ||
                    Input.GetAxisRaw("Vertical") > 0   && whereIsPlayer == Where.eUP ||
                    Input.GetAxisRaw("Vertical") < 0   && whereIsPlayer == Where.eDOWN)
                {
                    //if we tried pulling in a particular direction when there was something up ahead
                    if ((Input.GetAxisRaw("Horizontal") < 0 && somethingNearLeft ||
                         Input.GetAxisRaw("Horizontal") > 0 && somethingNearRight ||
                         Input.GetAxisRaw("Vertical") > 0   && somethingNearTop ||
                         Input.GetAxisRaw("Vertical") < 0   && somethingNearBottom) && playerHere)
                    {
                        //don't pull the cannon
                        return;
                    }

                    pulling = true;
                    pushing = false;

                    /*
                              [f_up]
                     [f_left][cannon][f_right]
                             [f_down]
                    */
                    //update the future positions based on the cannon's current position (by one unit)
                    f_right = new Vector2(transform.position.x + 1.0f, transform.position.y);
                    f_up = new Vector2(transform.position.x, transform.position.y + 1.0f);
                    f_left = new Vector2(transform.position.x - 1.0f, transform.position.y);
                    f_down = new Vector2(transform.position.x, transform.position.y - 1.0f);

                    //store the position the cannon was last in before the push
                    origin = transform.position;

                    //we are now clutching the cannon
                    //clutching = true;

                    //reset the move timer
                    moveTimer = 0.0f;

                    //have a temp to store the definitive direction the cannon will move
                    tempWhereIsPlayer = whereIsPlayer;
                }
                //if we pressed left while to the cannon's right
                //if we pressed right while to the cannon's left
                //if we pressed up while below the cannon
                //if we pressed down while above the cannon
                //if any of these conditions were met, we are pushing the cannon
                else if (Input.GetAxisRaw("Horizontal") < 0 && whereIsPlayer == Where.eRIGHT ||
                         Input.GetAxisRaw("Horizontal") > 0 && whereIsPlayer == Where.eLEFT ||
                         Input.GetAxisRaw("Vertical") > 0   && whereIsPlayer == Where.eDOWN ||
                         Input.GetAxisRaw("Vertical") < 0   && whereIsPlayer == Where.eUP)
                {
                    //if we tried pushing in a particular direction when there was something up ahead
                    if (Input.GetAxisRaw("Horizontal") < 0 && somethingOnLeft ||
                        Input.GetAxisRaw("Horizontal") > 0 && somethingOnRight ||
                        Input.GetAxisRaw("Vertical") > 0   && somethingOnTop ||
                        Input.GetAxisRaw("Vertical") < 0   && somethingOnBottom)
                    {
                        //don't push the cannon
                        return;
                    }

                    pushing = true;
                    pulling = false;

                    /*
                              [f_up]
                     [f_left][cannon][f_right]
                             [f_down]
                    */
                    //update the future positions based on the cannon's current position (by one unit)
                    f_right = new Vector2(transform.position.x + 1.0f, transform.position.y);
                    f_up = new Vector2(transform.position.x, transform.position.y + 1.0f);
                    f_left = new Vector2(transform.position.x - 1.0f, transform.position.y);
                    f_down = new Vector2(transform.position.x, transform.position.y - 1.0f);

                    //store the position the cannon was last in before the push
                    origin = transform.position;

                    //we are now clutching the cannon
                    //clutching = true;

                    //reset the move timer
                    moveTimer = 0.0f;

                    //have a temp to store the definitive direction the cannon will move
                    tempWhereIsPlayer = whereIsPlayer;
                }
            }
            //else if (Input.GetKeyUp(holdKey))
            //{
            //    heldOnto = false;
            //    inRange = true;
            //}
        }
        //else
        //{
        //    inRange = false;
        //    heldOnto = false;
        //    //player is not in range, so player is nowhere near the cannon to have a relative direction
        //    whereIsPlayer = Where.eNOWHERE;
        //}
    }

    private void Rotate()
    {
        //do not allow rotating if the cannon is in the middle of sinking, respawning, pushing, pulling, or a cannonball has been shot and is still in the scene
        if (sinking || transform.localScale != new Vector3(1, 1, 1) || pushing || pulling || transform.Find("DanielNunes_Cannonball(Clone)"))
        {
            return;
        }

        //in reality, playerPosition is the center of the player collider (which is near the legs)
        Vector3 playerPosition = new Vector3(player.transform.position.x - 0.15f, player.transform.position.y - 0.55f, 0);

        //only rotate when the player is in range of the cannon and it isn't already being pushed or pulled
        if (Input.GetKey(holdKey) && Vector3.Magnitude(transform.position - playerPosition) < proximity && moveTimer >= maxMoveTime)
        {
            ////if we pressed this key while we were not already rotating
            //if (!rotating && Input.GetKeyDown(rotateKey))
            //{
            //    //we are rotating
            //    rotating = true;
            //    rotateTimer = 0.0f;

            //    //save the original rotation of the cannon
            //    originalRot = transform.rotation.eulerAngles;
            //    //new rotation will be 90 degree counterclockwise
            //    newRot = originalRot + new Vector3(0, 0, 90.0f);
            //}

            //if we're not already rotating
            if (!rotating)
            {
                //if the player is to the left of the cannon and presses up
                //if the player is to the right of the cannon and presses down
                //if the player is above the cannon and presses right
                //if the pllayer is below the cannon and presses left
                //if ((whereIsPlayer == Where.eLEFT && Input.GetAxisRaw("Vertical") > 0) ||
                //    (whereIsPlayer == Where.eRIGHT && Input.GetAxisRaw("Vertical") < 0) ||
                //    (whereIsPlayer == Where.eUP && Input.GetAxisRaw("Horizontal") > 0) ||
                //    (whereIsPlayer == Where.eDOWN && Input.GetAxisRaw("Horizontal") < 0))
                if (Input.GetKey(KeyCode.E))
                {
                    //we are rotating
                    rotating = true;
                    rotateTimer = 0.0f;
                    //save the original rotation of the cannon
                    originalRot = transform.rotation.eulerAngles;
                    //new rotation will be 90 degree clockwise
                    newRot = originalRot - new Vector3(0, 0, 90.0f);

                    //store the last position of the player
                    playerLockPosition = player.transform.position;
                }
                //if the player is to the left of the cannon and presses down
                //if the player is to the right of the cannon and presses up
                //if the player is above the cannon and presses left
                //if the player is below the cannon and presses right
                //else if ((whereIsPlayer == Where.eLEFT && Input.GetAxisRaw("Vertical") < 0) ||
                //         (whereIsPlayer == Where.eRIGHT && Input.GetAxisRaw("Vertical") > 0) ||
                //         (whereIsPlayer == Where.eUP && Input.GetAxisRaw("Horizontal") < 0) ||
                //         (whereIsPlayer == Where.eDOWN && Input.GetAxisRaw("Horizontal") > 0))
                else if (Input.GetKey(KeyCode.Q))
                {
                    //we are rotating
                    rotating = true;
                    rotateTimer = 0.0f;
                    //save the original rotation of the cannon
                    originalRot = transform.rotation.eulerAngles;
                    //new rotation will be 90 degree counterclockwise
                    newRot = originalRot + new Vector3(0, 0, 90.0f);

                    //store the last position of the player
                    playerLockPosition = player.transform.position;
                }
            }
        }

        //while we rotate
        if (rotating)
        {
            //lerp the rotation from the original orientation to the new one
            transform.eulerAngles = Vector3.Lerp(originalRot, newRot, rotateTimer / maxMoveTime);
            //increment rotation timer
            rotateTimer += Time.deltaTime;
        }

        //if the rotation has finished
        if (rotateTimer >= maxMoveTime)
        {
            //stop the timer
            rotateTimer = maxMoveTime;
            //we are no longer rotating
            rotating = false;

            //there is a slight bit of error with lerping rotations, so to ensure the angle is perfect, assign it
            transform.eulerAngles = newRot;
        }
    }

    private void Sink()
    {
        //if we are only sinking
        if (!rotating && !pushing && !pulling && sinking && shrinkTimer < maxMoveTime)
        {
            //shrink the object
            transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), Vector3.zero, shrinkTimer / maxMoveTime);
            shrinkTimer += Time.deltaTime;
        }

        //the +1 is there to add some downtime between sinking and respawning
        if (sinking && shrinkTimer >= maxMoveTime && shrinkTimer < maxMoveTime + 1.0f)
        {
            //respawn the cannon at it's old origin
            transform.position = origin;
            //continue incrementing timer
            shrinkTimer += Time.deltaTime;
        }

        //once a second has passed anfter sinking
        if (sinking && shrinkTimer >= maxMoveTime + 1.0f)
        {
            //reset timer
            shrinkTimer = 0.0f;
            //cannon is no longer sinking
            sinking = false;
        }

        //if we have finished respawning
        if (!sinking && shrinkTimer >= maxMoveTime)
        {
            shrinkTimer = maxMoveTime;
            //re-enable all colliders on cannon
            GetComponent<BoxCollider2D>().enabled = true;
            transform.Find("Trigger").GetComponent<BoxCollider2D>().enabled = true;
            transform.localScale = new Vector3(1, 1, 1);

            ResetContacts();

            //prevents code below from being hit after respawning
            return;
        }
        
        //if we're not sinking, the timer hasn't maxed out, and we actually were shrinking before
        if (!sinking && shrinkTimer < maxMoveTime && transform.localScale != new Vector3(1, 1, 1))
        {
            //unshrink
            transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(1, 1, 1), shrinkTimer / maxMoveTime);
            shrinkTimer += Time.deltaTime;
        }
    }

    private void RaycastCheck()
    {
        //This is for preventing the player from pushing the cannon into walls or other similar tiles with physical colliders


        //send out a raycast to the right of the cannon
        RaycastHit2D[] rays = Physics2D.RaycastAll(transform.position, Vector2.right);

        //for each of the hits detected
        foreach (RaycastHit2D hit in rays)
        {
            //check to see if it's hitting something with a physical collider (NOT OURSELVES NOR THE PLAYER)
            if (AdditionalRaycastChecks(hit))
            {
                //if the distance of the ray is less than or equal to 1 unit, then it's right next to the cannon
                if (hit.distance <= 1.0f)
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    somethingOnRight = true;
                }
                //if the distance of the ray is less than or equal to 2 units, then there's a space between it and the cannon
                else if (hit.distance <= 2.0f)
                {
                    somethingNearRight = true;
                }
            }

            //if the player had been hit with the raycast
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerHere = true;
            }
        }

        //send out a raycast to the top of the cannon
        rays = Physics2D.RaycastAll(transform.position, Vector2.up);

        //for each of the hits detected
        foreach (RaycastHit2D hit in rays)
        {
            //check to see if it's hitting something with a physical collider (NOT OURSELVES NOR THE PLAYER)
            if (AdditionalRaycastChecks(hit))
            {
                //if the distance of the ray is less than or equal to 1 unit, then it's right next to the cannon
                if (hit.distance <= 1.0f)
                {
                    somethingOnTop = true;
                }
                //if the distance of the ray is less than or equal to 2 units, then there's a space between it and the cannon
                else if (hit.distance <= 2.0f)
                {
                    somethingNearTop = true;
                }
            }

            //if the player had been hit with the raycast
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerHere = true;
            }
        }

        //send out a raycast to the left of the cannon
        rays = Physics2D.RaycastAll(transform.position, -Vector2.right);

        //for each of the hits detected
        foreach (RaycastHit2D hit in rays)
        {
            //check to see if it's hitting something with a physical collider (NOT OURSELVES NOR THE PLAYER)
            if (AdditionalRaycastChecks(hit))
            {
                //if the distance of the ray is less than or equal to 1 unit, then it's right next to the cannon
                if (hit.distance <= 1.0f)
                {
                    somethingOnLeft = true;
                }
                //if the distance of the ray is less than or equal to 2 units, then there's a space between it and the cannon
                else if (hit.distance <= 2.0f)
                {
                    somethingNearLeft = true;
                }
            }

            //if the player had been hit with the raycast
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerHere = true;
            }
        }

        //send out a raycast to the bottom of the cannon
        rays = Physics2D.RaycastAll(transform.position, -Vector2.up);

        //for each of the hits detected
        foreach (RaycastHit2D hit in rays)
        {
            //check to see if it's hitting something with a physical collider (NOT OURSELVES NOR THE PLAYER)
            if (AdditionalRaycastChecks(hit))
            {
                //if the distance of the ray is less than or equal to 1 unit, then it's right next to the cannon
                if (hit.distance <= 1.0f)
                {
                    somethingOnBottom = true;
                }
                //if the distance of the ray is less than or equal to 2 units, then there's a space between it and the cannon
                else if (hit.distance <= 2.0f)
                {
                    somethingNearBottom = true;
                }
            }

            //if the player had been hit with the raycast
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerHere = true;
            }
        }
    }

    private bool AdditionalRaycastChecks(RaycastHit2D hit)
    {
        //returning true means we don't want the cannon to be moved

        //add as many checks as you want to this statement. If they all evaluate to true, then we ignore collision
        //if it isn't a trigger collider
        //if it isn't ourselves
        //if it isn't the player
        if (!hit.collider.isTrigger && !hit.collider.gameObject.name.Contains("Nunes_Cannon") &&
            !hit.collider.gameObject.CompareTag("Player"))
        {
            //if Taro's tiles exist in the scene
            if (FindObjectOfType<Taro_ColorSwitchManager>())
            {
                //if it's Taro's tile block
                if (hit.collider.gameObject.name.Contains("Taro_Tile"))
                {
                    //if the block is in the default collision layer
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Color_Blocks"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        return false;
    }

    public void ResetContacts()
    {
        //reset all contacts briefly
        somethingOnRight = false;
        somethingOnLeft = false;
        somethingOnTop = false;
        somethingOnBottom = false;

        somethingNearRight = false;
        somethingNearLeft = false;
        somethingNearTop = false;
        somethingNearBottom = false;

        playerHere = false;
    }

    private void SnapPlayer()
    {
        //get the offset of the player's collider, as that's technically the center point we're locking
        Vector2 offset = player.GetComponent<CircleCollider2D>().offset;

        //if player is horizontal to the cannon
        if (whereIsPlayer == Where.eRIGHT || whereIsPlayer == Where.eLEFT)
        {
            //snap them to the exact y position of the cannon, but maintain the x
            //player.transform.position = new Vector2(player.transform.position.x, transform.position.y - offset.y);

            if (whereIsPlayer == Where.eRIGHT)
                //snap them to the exact y position of the cannon and keep them a certain distance in the x
                player.transform.position = new Vector2(transform.position.x + 0.75f, transform.position.y - offset.y);
            else
                //snap them to the exact y position of the cannon and keep them a certain distance in the x
                player.transform.position = new Vector2(transform.position.x - 0.75f, transform.position.y - offset.y);
        }
        //if player is vertical to the cannon
        else
        {
            //get which way the player faces (scale.x = -1 means they're facing left)
            int scaleX = (int)player.transform.localScale.x;

            //snap them to the exact x position of the cannon, but maintain the y
            //player.transform.position = new Vector2(transform.position.x + (-scaleX * offset.x), player.transform.position.y);

            if (whereIsPlayer == Where.eUP)
                //snap them to the exact x position of the cannon and keep them a certain distance in the y
                player.transform.position = new Vector2(transform.position.x + (-scaleX * offset.x), transform.position.y + 1.4f);
            else
                //snap them to the exact x position of the cannon and keep them a certain distance in the y
                player.transform.position = new Vector2(transform.position.x + (-scaleX * offset.x), transform.position.y - 0.4f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if in the sand
        if (collision.gameObject.name.Contains("Nunes_TilemapSand"))
        {
            sinking = true;
            shrinkTimer = 0.0f;

            //disable all colliders on cannon
            GetComponent<BoxCollider2D>().enabled = false;
            transform.Find("Trigger").GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTarosTiles()
    {
        //this is to see if we're on top of one of Taro's swith blocks (ONLY IF IT'S ACTIVE)

        //send out a raycast to the right of the cannon
        RaycastHit2D[] rays = Physics2D.RaycastAll(transform.position, Vector2.right);

        foreach (RaycastHit2D hit in rays)
        {
            if (hit.collider.gameObject.name.Contains("Taro_Tilemap") && hit.distance < 0.25f)
            {
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("IgnorePlayerAndEnemy"))
                {
                    sinking = true;
                    shrinkTimer = 0.0f;

                    //disable all colliders on cannon
                    GetComponent<BoxCollider2D>().enabled = false;
                    transform.Find("Trigger").GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }

    private void FixPositioning()
    {
        //the cannons are offset by 0.5. Occasionally, there can be a glitch where moving the cannon doesn't
        //fully move it (i.e. from x = 0.5 to 1.5, but lerp screws up and stops at 1.4, which is potentially bad)
        //so, we make sure the tenths place is always a 5

        bool fineX = false;
        bool fineY = false;

        Vector3 position = transform.position;

        //fix the x position

        float newX = 0.0f;

        // 1.4 - 1 = 0.4
        float x_decimal = Mathf.Abs(position.x) - (int)Mathf.Abs(position.x);
        // 4
        int x_integer = (int)(x_decimal * 10.0f);
        // 4 != 5
        if (x_integer != 5)
        {
            //if positive
            if (position.x >= 0)
            {
                newX = (float)((int)position.x) + 0.5f; 
            }
            //if negative
            else
            {
                newX = (float)((int)position.x) - 0.5f;
            }
        }
        else
        {
            fineX = true;
        }

        //fix the y position

        float newY = 0.0f;

        // 1.4 - 1 = 0.4
        float y_decimal = Mathf.Abs(position.y) - (int)Mathf.Abs(position.y);
        // 4
        int y_integer = (int)(y_decimal * 10.0f);
        // 4 != 5
        if (y_integer != 5)
        {
            //if positive
            if (position.y >= 0)
            {
                newY = (float)((int)position.y) + 0.5f;
            }
            //if negative
            else
            {
                newY = (float)((int)position.y) - 0.5f;
            }
        }
        else
        {
            fineY = true;
        }


        //if the values were already fine
        if (fineX && fineY)
        {
            return;
        }
        //if only the x position was screwed up
        else if (!fineX && fineY)
        {
            transform.position = new Vector2(newX, transform.position.y);
        }
        //if only the y position was screwed up
        else if (!fineY && fineX)
        {
            transform.position = new Vector2(transform.position.x, newY);
        }
        //if both x and y were screwed up
        else
        {
            transform.position = new Vector2(newX, newY);
        }
    }
}
