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

    Vector2 Target;
    private Transform playerTrans;

    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        Target = new Vector2(playerTrans.position.x, playerTrans.position.y);

        if(playerTrans.localScale.x < 0)
        {
            Target.x -= 20;
        }
        else
        {
            Target.x += 20;
            FireballRenderer.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Update()
    {        
        transform.position = Vector2.MoveTowards(transform.position, Target, Speed* Time.deltaTime);
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
