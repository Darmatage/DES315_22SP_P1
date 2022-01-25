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


    // Start is called before the first frame update
    void Start()
    {
        if (transform.root.gameObject.tag == "Player")
            IsAttachedToPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAttachedToPlayer)
            return;
        if(IsHoldingEnemy)
        {
            projectileClone.transform.position = transform.position;

            if(Input.GetMouseButton(1))
            {
                IsHoldingEnemy = false;
                projectileClone.GetComponent<BraedanProjectile>().isHeld = false;
                Rigidbody2D rigidbody2D = projectileClone.GetComponent<Rigidbody2D>();
                Vector2 direction;
                direction.x = Input.GetAxisRaw("Horizontal");
                direction.y = Input.GetAxisRaw("Vertical");
                direction.Normalize();
                rigidbody2D.AddTorque(223);
                if(direction.x != 0.0f || direction.y != 0.0f)
                    rigidbody2D.velocity = (direction * projectileSpeed);
                else
                {
                    if (transform.root.localScale.x == 1)
                        rigidbody2D.velocity = Vector2.right * projectileSpeed;
                    else
                        rigidbody2D.velocity = Vector2.left * projectileSpeed;
                }
            }

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