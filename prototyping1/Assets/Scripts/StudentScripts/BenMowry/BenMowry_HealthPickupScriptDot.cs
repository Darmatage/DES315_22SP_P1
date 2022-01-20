using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenMowry_HealthPickupScriptDot : MonoBehaviour
{
    GameHandler gameHandler;

    public int healthAmount = 0;

    //Time to reach healthAmount
    public float time = 0.0f;

    //Amount of health per tick, must be less than healthAmount
    public int healthPerTick = 0;

    void Awake()
    {
        gameHandler = FindObjectOfType<GameHandler>();

        if(healthPerTick > healthAmount)
            healthPerTick = healthAmount;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(GameHandler.PlayerHealth < gameHandler.PlayerHealthStart)
            {
                Destroy(gameObject);
                //Dot logic
            }
        }
    }
}
