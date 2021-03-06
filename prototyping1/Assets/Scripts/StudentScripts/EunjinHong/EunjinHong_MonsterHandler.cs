using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EunjinHong_MonsterHandler : MonoBehaviour
{
    public int MonsterHealth = 5;
    public int MonsterStartHealth = 5;
    private bool IsDead = false;
    public GameObject Monster;

    public Renderer Rend;


    private EunjinHong_GameHandler gameHandlerObj;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("GameHandler") != null)
        {
            gameHandlerObj = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<EunjinHong_GameHandler>();
        }

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
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
            if(gameHandlerObj.Stamina < gameHandlerObj.StaminaStart)
            {
                gameHandlerObj.ChargeStamina(1);
            }

            IsDead = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
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
