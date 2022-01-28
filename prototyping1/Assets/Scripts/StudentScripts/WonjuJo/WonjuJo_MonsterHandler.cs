using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonjuJo_MonsterHandler : MonoBehaviour
{
    public int MonsterHealth = 100;
    public int MonsterStartHealth = 100;
    private bool IsDead = false;
    public GameObject Monster;

    public Renderer Rend;

    public WonjuJo_PlayerMovement PM;

    // Start is called before the first frame update
    void Start()
    {
        if (!PM)
        {
            Debug.Log("[WonjuJo_MonsterHandler] - There is no PlayerMovement reference.");

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                WonjuJo_PlayerMovement pm = player.GetComponent<WonjuJo_PlayerMovement>();

                if (pm != null)
                {
                    PM = pm;
                    Debug.Log("[WonjuJo_MonsterHandler] - Found PlayerMovement reference.");
                }
            }
        }
    }


    public void MonsterTakeDamge(int damage)
    {
        StopCoroutine(ChangeColor());
        StartCoroutine(ChangeColor());

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
        if(IsDead)
        {
            Destroy(Monster);
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
