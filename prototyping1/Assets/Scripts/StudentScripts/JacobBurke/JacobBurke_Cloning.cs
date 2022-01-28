using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacobBurke_Cloning : MonoBehaviour
{
    public float lifeDis = 10f;

    public GameObject shadowSpawner;
    public bool pushClone = false;

    private Vector3 change; // player movement direction
    private Rigidbody2D rb2d;
    private Rigidbody2D playerRb2d;
    private Animator anim;
    private bool isAlive = true;
    private float speed; // player movement speed

    GameObject player;
    private float ogPlayerSpeed;

    ParticleSystem shadowParticles;

    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ogPlayerSpeed = player.GetComponent<PlayerMove>().speed;
        speed = ogPlayerSpeed;
        anim = gameObject.GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<Renderer>();
        shadowParticles = GetComponentInChildren<ParticleSystem>();

        if (gameObject.GetComponent<Rigidbody2D>() != null)
            rb2d = GetComponent<Rigidbody2D>();

        if (player.GetComponent<Rigidbody2D>() != null)
            playerRb2d = player.GetComponent<Rigidbody2D>();

        rb2d.drag = 1500;
        playerRb2d.drag = 1500;
    }

    void FixedUpdate()
    {
        if (isAlive == true)
        {
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

            if (Input.GetKey(KeyCode.LeftShift))
            {
                player.GetComponent<PlayerMove>().speed = 0;
                rb2d.bodyType = RigidbodyType2D.Dynamic;

                if(isAlive && !pushClone)
                    playerRb2d.bodyType = RigidbodyType2D.Static;

                change = Vector3.zero;
                change.x = Input.GetAxisRaw("Horizontal");
                change.y = Input.GetAxisRaw("Vertical");
                UpdateAnimationAndMove();

            }
            else
            {
                player.GetComponent<PlayerMove>().speed = ogPlayerSpeed;
                anim.SetBool("Walk", false);

                if(!pushClone)
                    rb2d.bodyType = RigidbodyType2D.Static;

                playerRb2d.bodyType = RigidbodyType2D.Dynamic;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                anim.SetTrigger("Attack");
            }
        }
    }

    void UpdateAnimationAndMove()
    {
        if (isAlive == true)
        {
            if (change != Vector3.zero && speed != 0)
            {
                rb2d.MovePosition(transform.position + change * speed * Time.deltaTime);
                anim.SetBool("Walk", true);
            }
            else
            {
                anim.SetBool("Walk", false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If certain distance, vanish
        if (!GetComponent<Renderer>().isVisible)
            DestroyClone();

        if (Input.GetKeyDown(KeyCode.R))
            DestroyClone();

        //E to teleport
        if(Input.GetKeyDown(KeyCode.E))
        {
            Vector3 pos = transform.position;
            transform.position = player.transform.position;
            player.transform.position = pos;
            shadowParticles.Play();
        }
    }

    void DestroyClone()
    {
        isAlive = false;
        shadowSpawner.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<PlayerMove>().speed = ogPlayerSpeed;
        shadowParticles.Play();
        playerRb2d.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, 0.5f);
    }
}
