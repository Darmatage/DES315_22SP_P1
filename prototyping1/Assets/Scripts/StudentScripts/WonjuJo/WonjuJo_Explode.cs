using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonjuJo_Explode : MonoBehaviour
{
    public WonjuJo_ExplosionEnemy ExplosionEnemy;

    private WonjuJo_PlayerHandler gameHandlerObj;

    // Start is called before the first frame update

    private bool Once = false;

    void Start()
    {
        //ExplosionEnemy = GetComponent<WonjuJo_ExplosionEnemy>();

        if (!ExplosionEnemy)
            Debug.Log("There is no explosion enemy");

        GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
        if (gameHandlerLocation != null)
        {
            gameHandlerObj = gameHandlerLocation.GetComponent<WonjuJo_PlayerHandler>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (ExplosionEnemy.GetIsDead() && !Once)
        {
            StartCoroutine("Delay");
            gameHandlerObj.TakeDamage(20);
            Once = true;

        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
    }
}
