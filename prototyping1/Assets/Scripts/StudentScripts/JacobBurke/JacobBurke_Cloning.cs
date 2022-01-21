using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacobBurke_Cloning : MonoBehaviour
{
    public float lifeDis = 10f;
    public float summonTime = 100.0f;

    private Vector3 change; // player movement direction
    private Rigidbody2D rb2d;
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
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
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
                change = Vector3.zero;
                change.x = Input.GetAxisRaw("Horizontal");
                change.y = Input.GetAxisRaw("Vertical");
                UpdateAnimationAndMove();

            }
            else
            {
                player.GetComponent<PlayerMove>().speed = ogPlayerSpeed;
                anim.SetBool("Walk", false);
                rb2d.bodyType = RigidbodyType2D.Static;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                anim.SetTrigger("Attack");
            }
        } //else playerDie(); //run this function from the GameHandler instead
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
        if (Vector3.Distance(player.transform.position, transform.position) > lifeDis)
            DestroyClone();

        //If certain time, vanish
        //if (summonTime > 0)
            //summonTime -= Time.deltaTime;
        //else
            //DestroyClone();

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
        player.GetComponent<PlayerMove>().speed = ogPlayerSpeed;
        shadowParticles.Play();
        Destroy(gameObject, 0.5f);
    }
}
