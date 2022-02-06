using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraedanNeversGrabEnemy : MonoBehaviour
{
    public float projectileSpeed;
    private bool IsAttachedToPlayer = false;
    private bool IsHoldingEnemy = false;
    private bool EnemyInRange = false;


    public GameObject projectilePrefab;
    
    private GameObject projectileClone = null;
    private Vector2 direction;

    private GameObject grabCircle; 
    private SpriteRenderer uLine;
    private SpriteRenderer rLine;
    private SpriteRenderer lLine;
    private SpriteRenderer dLine;
    private bool directionSet = false;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.root.gameObject.tag == "Player")
        {
            IsAttachedToPlayer = true;
            direction = Vector2.zero;
            grabCircle = transform.GetChild(0).gameObject;
            uLine = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
            rLine = transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
            lLine = transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>();
            dLine = transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAttachedToPlayer)
            return;
        if(IsHoldingEnemy)
        {
            projectileClone.transform.position = transform.position;

            grabCircle.SetActive(false);
            lLine.gameObject.SetActive(true);
            rLine.gameObject.SetActive(true);
            dLine.gameObject.SetActive(true);
            uLine.gameObject.SetActive(true);

            Color lineColor = new Color(rLine.color.r, rLine.color.g, rLine.color.b, 1.0f);
            Color transparent = new Color(rLine.color.r, rLine.color.b, rLine.color.b, 0.5f);

            if (!directionSet)
            {
            }


            if (Input.GetKey(KeyCode.W))
            {
                direction = Vector2.up;
                uLine.color = lineColor;
                rLine.color = transparent;
                lLine.color = transparent;
                dLine.color = transparent;
            }
            if(Input.GetKey(KeyCode.S))
            {
                direction = Vector2.down;
                dLine.color = lineColor;
                rLine.color = transparent;
                uLine.color = transparent;
                lLine.color = transparent;
            }
            if(Input.GetKey(KeyCode.D))
            {
                direction = Vector2.right;
                rLine.color = lineColor;
                lLine.color = transparent;
                // Throw up-right
                if (Input.GetKey(KeyCode.W))
                {
                    uLine.color = lineColor;
                    dLine.color = transparent;
                    direction += Vector2.up;
                }
                // Throw down-right
                else if (Input.GetKey(KeyCode.S))
                {
                    uLine.color = transparent;
                    dLine.color = lineColor;
                    direction += Vector2.down;
                }
                // Just right
                else
                {
                    uLine.color = transparent;
                    dLine.color = transparent;
                }
            }
            if(Input.GetKey(KeyCode.A))
            {
                // Throw left
                direction = Vector2.left;
                rLine.color = lineColor;    // We're setting the rline because the sprite is inverted when facing left
                lLine.color = transparent;

                // Throw up-left
                if(Input.GetKey(KeyCode.W))
                {
                    uLine.color = lineColor;
                    dLine.color = transparent;
                    direction += Vector2.up;
                }
                // Throw down-Left
                else if(Input.GetKey(KeyCode.S))
                {
                    uLine.color = transparent;
                    dLine.color = lineColor;
                    direction += Vector2.down;
                }
                // Just left
                else
                {
                    uLine.color = transparent;
                    dLine.color = transparent;
                }

            }
            
                // Throw the enemy
            if(Input.GetMouseButton(1))
            {
                if(direction == Vector2.zero)
                {
                    rLine.color = lineColor;
                    lLine.color = transparent;
                    uLine.color = transparent;
                    dLine.color = transparent;
                    if (transform.root.localScale.x == 1)
                        direction = Vector2.right;
                    else
                        direction = Vector2.left; 

                }    

                IsHoldingEnemy = false;
                projectileClone.GetComponent<BraedanProjectile>().isHeld = false;
                Rigidbody2D rigidbody2D = projectileClone.GetComponent<Rigidbody2D>();
                
                rigidbody2D.AddTorque(223);
                
                rigidbody2D.velocity = direction * projectileSpeed;
            }
        }
        else
        {
            grabCircle.SetActive(true);
            lLine.gameObject.SetActive(false);
            rLine.gameObject.SetActive(false);
            uLine.gameObject.SetActive(false);
            dLine.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer == 6 && IsAttachedToPlayer)
        {
            if(Input.GetMouseButton(0) && !IsHoldingEnemy)
            {
                IsHoldingEnemy = true;
                EnemyInRange = false;   
                projectileClone = Instantiate(projectilePrefab, transform.position, transform.rotation);
                projectileClone.GetComponent<BraedanProjectile>().isHeld = true;

                projectileClone.transform.localScale = other.transform.localScale;

                SpriteRenderer projectileSpriteRenderer = projectileClone.transform.GetChild(0).GetComponent<SpriteRenderer>();
                SpriteRenderer enemySpriteRenderer = other.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
                projectileSpriteRenderer.sprite = enemySpriteRenderer.sprite;
                projectileSpriteRenderer.color = enemySpriteRenderer.color;

                Destroy(other.gameObject);

            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            EnemyInRange = true;   
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            EnemyInRange = false;
        }

    }
}