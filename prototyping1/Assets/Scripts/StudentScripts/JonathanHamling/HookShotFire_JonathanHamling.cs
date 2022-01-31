using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShotFire_JonathanHamling : MonoBehaviour
{
    LineRenderer line;

    [SerializeField] 
    private LayerMask whatCanGrapple;

    [SerializeField]
    private float shootSpeed = 20f;
    [SerializeField] 
    private float maxDistance = 10f;
    [SerializeField] 
    private float retractSpeed = 10f;

    [HideInInspector]
    public bool isRetract = false;
    [HideInInspector]
    public bool isPull = false;
    private bool isGrappled = false;
<<<<<<< HEAD
    private bool isLaunched = false;
=======
<<<<<<< Updated upstream
>>>>>>> 4ffb102 (Updated Level - Changes to MageHand)
    
=======
    private bool isLaunched = false;

    Vector2 mousPos;
>>>>>>> Stashed changes
    Vector2 target;
    GameObject targetObj;

    [SerializeField] 
    private GameObject Player;
<<<<<<< HEAD
    [SerializeField]
    private Transform positionRot;
=======
<<<<<<< Updated upstream
=======
    [SerializeField]
    private GameObject handImage;
    [SerializeField]
    private Transform positionRot;
>>>>>>> Stashed changes

    public Sprite[] hands;
    public SpriteRenderer handRend;
>>>>>>> 4ffb102 (Updated Level - Changes to MageHand)


    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
<<<<<<< HEAD
        if (Input.GetButtonDown("Fire1") && !isGrappled)
=======
<<<<<<< Updated upstream
        if (Input.GetMouseButtonDown(0) && !isGrappled)
=======
        mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 player = Player.transform.position;

        if (Vector2.Distance(mouse, player) < maxDistance && (!isPull && !isRetract && !isGrappled))
        {
            handImage.SetActive(true);
            handRend.sprite = hands[0];
            handImage.transform.position = mousPos;
        }
        else if (Vector2.Distance(mouse, player) > maxDistance && (!isPull && !isRetract && !isGrappled))
        {
            handImage.SetActive(false);
        }
            
        if (Input.GetButtonDown("Fire1") && !isGrappled)
>>>>>>> Stashed changes
>>>>>>> 4ffb102 (Updated Level - Changes to MageHand)
        {
            Grapple();
        }

    }

    private void FixedUpdate()
    {
        // Lets pull back now, (if we can), fixed update becuz reasons.
        if (isRetract)
        {
            Vector2 grapplePos = Vector2.Lerp(Player.transform.position, target, retractSpeed * Time.deltaTime);

            handImage.SetActive(true);
            handImage.transform.position = target;
            handRend.sprite = hands[2];
            // setting new player transform to lerp!
            Player.transform.position = grapplePos;

            // draw the new line :)
            line.SetPosition(0, transform.position);

            if (Vector2.Distance(Player.transform.position, target) < 1f)
            {
                isRetract = false;
                isGrappled = false;
                handImage.SetActive(false);

                // are we ready to stop the rope?
                line.enabled = false;
            }
        }
        else if (isPull)
        {
<<<<<<< HEAD
            Vector2 grapplePos = Vector2.Lerp(targetObj.transform.position, transform.position, retractSpeed * Time.deltaTime);
=======
<<<<<<< Updated upstream
            Vector2 grapplePos = Vector2.Lerp(targetObj.transform.position, transform.position, shootSpeed * Time.deltaTime);
>>>>>>> 4ffb102 (Updated Level - Changes to MageHand)

            targetObj.transform.position = grapplePos;
=======
            // mouse position on screen
            // Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Vector2 direction = targetObj.transform.position - transform.position;
>>>>>>> Stashed changes

            // Raycast for grapple
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, whatCanGrapple);

<<<<<<< HEAD
            if ((Vector2.Distance(transform.position, targetObj.transform.position) < .5f) && !Input.GetButton("Fire1"))
=======
<<<<<<< Updated upstream
            if (Vector2.Distance(transform.position, targetObj.transform.position) < .5f)
=======
            if (hit.collider != null)
>>>>>>> Stashed changes
            {
                // Is it grappleable?
                if (hit.collider.tag == "Pullable")
                {

<<<<<<< Updated upstream
                // are we ready to stop the rope?
                line.enabled = false;
            }
=======
                    Vector2 grapplePos = Vector2.Lerp(targetObj.transform.position, mousPos, shootSpeed * Time.deltaTime);

                    targetObj.transform.position = grapplePos;

                    line.SetPosition(1, targetObj.transform.position);
                    handImage.transform.position = mousPos;
                    handRend.sprite = hands[1];

                    if ((Vector2.Distance(mousPos, targetObj.transform.position) < .25f) && !Input.GetButton("Fire1"))
                    {
                        isPull = false;
                        isGrappled = false;

                        // are we ready to stop the rope?
                        line.enabled = false;
                        handImage.SetActive(false);
                        handRend.sprite = hands[0];
                    }
                    else if (Vector2.Distance(mousPos, targetObj.transform.position) < .25f)
                    {

                        if (Input.GetButton("Fire2"))
                        {
                            if (targetObj.GetComponent<Rigidbody2D>())
                            {
                                isLaunched = true;

                                isPull = false;
                                isGrappled = false;
                            }
                        }

                        line.enabled = false;
                    }
                }
                else
                {
                    isPull = false;
                    isGrappled = false;

                    // are we ready to stop the rope?
                    line.enabled = false;
                    handImage.SetActive(false);
                    handRend.sprite = hands[0];
                }
            }
            else
>>>>>>> 4ffb102 (Updated Level - Changes to MageHand)
            {
                isPull = false;
                isGrappled = false;

                // are we ready to stop the rope?
                line.enabled = false;
                handImage.SetActive(false);
                handRend.sprite = hands[0];
            }
            else if (Vector2.Distance(transform.position, targetObj.transform.position) < .5f)
            {

                if (Input.GetButton("Fire2"))
                {
                    if (targetObj.GetComponent<Rigidbody2D>())
                    {
                        isLaunched = true;

                        isPull = false;
                        isGrappled = false;
                    }
                }

                line.enabled = false;
            }
        }

        if (isLaunched == true)
        {
            targetObj.GetComponent<Rigidbody2D>().AddForce(positionRot.right * shootSpeed, ForceMode2D.Impulse);

            isLaunched = false;
        }

        if (isLaunched == true)
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Player.transform.position;

            direction.Normalize();

            float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            targetObj.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

            targetObj.GetComponent<Rigidbody2D>().AddForce(targetObj.transform.right * shootSpeed, ForceMode2D.Impulse);

            isLaunched = false;
>>>>>>> Stashed changes
        }
    }

    private void Grapple()
    {
        // mouse position on screen
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // Raycast for grapple
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, whatCanGrapple);

        if (hit.collider != null)
        {
            // Is it grappleable?
            if (hit.collider.tag == "Grappleable")
            {
                // Sets the point to move to
                target = hit.point;

                // Grappled is now definitely true
                isGrappled = true;

                // Readies line to be used by subroutine
                line.enabled = true;
                line.positionCount = 2;

                StartCoroutine(Retract());
            }
            else if (hit.collider.tag == "Pullable")
            {
                // Sets the point to pull from
                target = hit.collider.transform.position;
                targetObj = hit.collider.gameObject;

                // Grappled is now definitely true
                isGrappled = true;

                // Readies line to be used by subroutine
                line.enabled = true;
                line.positionCount = 2;

                StartCoroutine(Pull());
            }
        }
    }

    IEnumerator Pull()
    {
        float i = 0;

        // Wooo its a line!
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        Vector2 newPos;
        Vector2 distance = transform.position - targetObj.transform.position;

        handImage.SetActive(true);
        handRend.sprite = hands[3];

        // Gotta lerp this real quick
        for (; i < distance.magnitude; i += shootSpeed * Time.deltaTime)
        {
<<<<<<< HEAD
            newPos = Vector2.Lerp(transform.position, targetObj.transform.position, i / distance.magnitude);
=======
<<<<<<< Updated upstream
            newPos = Vector2.Lerp(transform.position, target, i / timeLerped);
=======
            newPos = Vector2.Lerp(mousPos, targetObj.transform.position, i / distance.magnitude);
>>>>>>> Stashed changes
>>>>>>> 4ffb102 (Updated Level - Changes to MageHand)

            line.SetPosition(0, mousPos);
            line.SetPosition(1, newPos);
            
            handImage.transform.position = mousPos;

            yield return null;
        }

        line.SetPosition(1, target);
        isPull = true;
    }

    IEnumerator Retract()
    {
        float i = 0;

        // Wooo its a line!
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        Vector2 newPos;
        Vector2 distance = new Vector2(transform.position.x, transform.position.y) - target;

        handImage.SetActive(true);
        handRend.sprite = hands[2];

        // Gotta lerp this real quick
        for (; i < distance.magnitude; i += shootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(transform.position, target, i / distance.magnitude);

            line.SetPosition(0, transform.position);
            line.SetPosition(1, newPos);

            handImage.transform.position = target;

            yield return null;
        }

        line.SetPosition(1, target);
        isRetract = true;
    }
}
