using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonjuJo_Explode : MonoBehaviour
{
    public WonjuJo_ExplosionEnemy ExplosionEnemy;

    private WonjuJo_PlayerHandler gameHandlerObj;

    //private WonjuJo_MonsterHandler MH;
    // Start is called before the first frame update

    private bool Once = false;
    private bool Hit = false;

    private Transform playerTrans;


    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        if (!ExplosionEnemy)
        {
            Debug.Log("There is no explosion enemy");
        }

        GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");

        if (gameHandlerLocation != null)
        {
            gameHandlerObj = gameHandlerLocation.GetComponent<WonjuJo_PlayerHandler>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Hit = true;

            if (ExplosionEnemy.GetIsDead() && !Once)
            {
                Once = true;

                StartCoroutine(Delay(1.8f));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Hit = false;
        }
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);

        if (Hit)
        {
            gameHandlerObj.TakeDamage(20);
        }
    }
}