using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KobeDennis_LavaBallScript : MonoBehaviour
{
    public float speed;
    public float LifeTime = 0f;
    private Rigidbody2D rb;
    private Vector3 direction;
    
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
    private void FixedUpdate()
    {
        rb.velocity = (Vector2)direction * speed;
    }
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, LifeTime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //StartCoroutine(collision.collider.gameObject.GetComponent<MonsterMoveHit>().GetHit);


            Destroy(gameObject);
        }
        
    }
}
