using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EunjinHong_PlayerMove : MonoBehaviour
{
    //NOTE: This script has been adjusted to make all functions virtual, allowing for derived classes to override functions


    public float speed = 3f; // player movement speedf
    public Vector3 change; // player movement direction
    private Rigidbody2D rb2d;
    private Animator anim;
    private bool isAlive = true;

    private Renderer rend;


    public bool isDashing;
    private Vector3 moveDir;

    [SerializeField] private LayerMask dashLayerMask;


    protected virtual void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<Renderer>();

        if (gameObject.GetComponent<Rigidbody2D>() != null)
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if (isAlive == true)
        {
            change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");
            UpdateAnimationAndMove();

            moveDir = new Vector3(change.x, change.y).normalized;

            if (Input.GetAxis("Horizontal") > 0)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = 1.0f;
                transform.localScale = newScale;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = -1.0f;
                transform.localScale = newScale;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                anim.SetTrigger("Attack");
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isDashing = true;
            }
        } //else playerDie(); //run this function from the GameHandler instead

    }
    protected virtual void FixedUpdate()
    {

        if (isDashing)
        {
            float dashAmount = 3f;
            Vector3 dashPosition = transform.position + moveDir * dashAmount;

            RaycastHit2D raycastHit2d = Physics2D.Raycast(transform.position, moveDir, dashAmount, dashLayerMask);
            if (raycastHit2d.collider != null)
            {
                dashPosition = raycastHit2d.point;
            }

            rb2d.MovePosition(dashPosition);
            isDashing = false;
        }
        //if (isAlive == true)
        //{
        //    change = Vector3.zero;
        //    change.x = Input.GetAxisRaw("Horizontal");
        //    change.y = Input.GetAxisRaw("Vertical");
        //    UpdateAnimationAndMove();

        //    if (Input.GetAxis("Horizontal") > 0)
        //    {
        //        Vector3 newScale = transform.localScale;
        //        newScale.x = 1.0f;
        //        transform.localScale = newScale;
        //    }
        //    else if (Input.GetAxis("Horizontal") < 0)
        //    {
        //        Vector3 newScale = transform.localScale;
        //        newScale.x = -1.0f;
        //        transform.localScale = newScale;
        //    }

        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        anim.SetTrigger("Attack");
        //    }


        //    if (isDashing)
        //    {
        //        float dashAmount = 50f;
        //        Vector3 dashPosition = transform.localScale + moveDir * dashAmount;

        //        RaycastHit2D raycastHit2d = Physics2D.Raycast(transform.localScale, moveDir, dashAmount, dashLayerMask);
        //        if (raycastHit2d.collider != null)
        //        {
        //            dashPosition = raycastHit2d.point;
        //        }

        //        rb2d.MovePosition(dashPosition);
        //        isDashing = false;
        //    }
        //} //else playerDie(); //run this function from the GameHandler instead
    }


    protected virtual void UpdateAnimationAndMove()
    {
        if (isAlive == true)
        {
            if (change != Vector3.zero && speed != 0)
            {
                rb2d.MovePosition(transform.position + change * speed * Time.deltaTime);
                //MoveCharacter();
                //anim.SetFloat("moveX", change.x);
                //anim.SetFloat("moveY", change.y);
                anim.SetBool("Walk", true);
            }
            else
            {
                anim.SetBool("Walk", false);
            }
        }
    }

    public virtual void playerHit()
    {
        if (isAlive == true)
        {
            anim.SetTrigger("Hurt");
            StopCoroutine(ChangeColor());
            StartCoroutine(ChangeColor());
        }
    }

    public virtual void playerDie()
    {
        anim.SetBool("isDead", true);
        if (isAlive == false)
        {
            //Debug.Log("I'm already dead");
        }
        else if (isAlive == true)
        {
            isAlive = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            //gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }


    protected virtual IEnumerator ChangeColor()
    {
        // color values are R, G, B, and alpha, each 0-255 divided by 100
        rend.material.color = new Color(2.0f, 1.0f, 0.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        rend.material.color = Color.white;
    }

}

