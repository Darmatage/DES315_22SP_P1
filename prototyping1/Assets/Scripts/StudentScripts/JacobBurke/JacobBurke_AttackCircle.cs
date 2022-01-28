using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacobBurke_AttackCircle : MonoBehaviour
{
    public int damage = 1;
    private float liveTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(liveTime >= 0)
        {
            liveTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "monsterShooter")
        {
            collision.gameObject.GetComponent<EnemyHealth>().HitEnemy();
        }
    }
}
