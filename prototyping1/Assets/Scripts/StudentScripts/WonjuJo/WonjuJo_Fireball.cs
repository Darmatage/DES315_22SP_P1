using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonjuJo_Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    public float FireballLife = 3f;
    private float FireballTimer;
    public float Speed = 5f;
    public int Damage = 15;

    public Renderer FireballRenderer;

    private Transform playerTrans;
    Vector3 direction;

    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }

    private void Update()
    {       
        transform.position = Vector2.MoveTowards(transform.position, direction, Speed* Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            WonjuJo_MonsterHandler MH = collision.GetComponent<WonjuJo_MonsterHandler>();
            MH.MonsterTakeDamge(15);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "ExplodeEnemy")
        {
            WonjuJo_ExplosionEnemy Explosion = collision.GetComponent<WonjuJo_ExplosionEnemy>();
            Explosion.MonsterTakeDamge(15);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        FireballTimer += 0.1f;
        if(FireballTimer >= FireballLife)
        {
            Destroy(gameObject);
        }
    }
}
