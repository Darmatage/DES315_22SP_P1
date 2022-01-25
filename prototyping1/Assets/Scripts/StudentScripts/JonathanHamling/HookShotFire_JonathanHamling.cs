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
    private bool isLaunched = false;
    
    Vector2 target;
    GameObject targetObj;

    [SerializeField] 
    private GameObject Player;
    [SerializeField]
    private Transform positionRot;


    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isGrappled)
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

            // setting new player transform to lerp!
            Player.transform.position = grapplePos;

            // draw the new line :)
            line.SetPosition(0, transform.position);

            if (Vector2.Distance(Player.transform.position, target) < 1.5f)
            {
                isRetract = false;
                isGrappled = false;
                
                // are we ready to stop the rope?
                line.enabled = false;
            }
        }
        else if (isPull)
        {
            Vector2 grapplePos = Vector2.Lerp(targetObj.transform.position, transform.position, retractSpeed * Time.deltaTime);

            targetObj.transform.position = grapplePos;

            line.SetPosition(1, targetObj.transform.position);

            if ((Vector2.Distance(transform.position, targetObj.transform.position) < .5f) && !Input.GetButton("Fire1"))
            {
                isPull = false;
                isGrappled = false;

                // are we ready to stop the rope?
                line.enabled = false;
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

        // Gotta lerp this real quick
        for (; i < distance.magnitude; i += shootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(transform.position, targetObj.transform.position, i / distance.magnitude);

            line.SetPosition(0, transform.position);
            line.SetPosition(1, newPos);

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

        // Gotta lerp this real quick
        for (; i < distance.magnitude; i += shootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(transform.position, target, i / distance.magnitude);

            line.SetPosition(0, transform.position);
            line.SetPosition(1, newPos);

            yield return null;
        }

        line.SetPosition(1, target);
        isRetract = true;
    }
}
