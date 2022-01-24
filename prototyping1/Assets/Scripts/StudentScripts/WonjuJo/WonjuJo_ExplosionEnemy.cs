using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonjuJo_ExplosionEnemy : MonoBehaviour
{
    public int MonsterHealth = 100;
    public int MonsterStartHealth = 100;
    private bool IsDead = false;
    public GameObject ExplosionEnemy;

    public float ExplosionDelay = 2.0f;

    public Renderer Rend;

    public WonjuJo_PlayerMovement PM;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!PM)
            Debug.Log("There is no PM");
    }

    public bool GetIsDead() { return IsDead; }

    public void MonsterTakeDamge(int damage)
    {
        MonsterHealth -= damage;
        if (MonsterHealth <= 0)
        {
            MonsterHealth = 0;
            IsDead = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        { 
            Destroy(ExplosionEnemy, ExplosionDelay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            StopCoroutine(ChangeColor());
            StartCoroutine(ChangeColor());
        }

        if (collision.gameObject.tag == "Player" && PM.GetIsAttack())
        {
            StopCoroutine(ChangeColor());
            StartCoroutine(ChangeColor());
        }
    }

    IEnumerator ChangeColor()
    {
        // color values are R, G, B, and alpha, each 0-255 divided by 100
        Rend.material.color = new Color(2.0f, 1.0f, 0.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Rend.material.color = Color.white;
    }


}
