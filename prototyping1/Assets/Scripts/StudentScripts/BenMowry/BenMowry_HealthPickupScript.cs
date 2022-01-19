using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenMowry_HealthPickupScript : MonoBehaviour
{
    GameHandler gameHandler;

    public int healthAmount = 0;

    void Awake()
    {
        gameHandler = FindObjectOfType<GameHandler>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(gameHandler.PlayerHealth < gameHandler.PlayerHealthStart)
        {
            Destroy(gameObject);

            if((gameHandler.PlayerHealth + healthAmount) > gameHandler.PlayerHealthStart)
                gameHandler.PlayerHealth = gameHandler.PlayerHealthStart;
            else
                gameHandler.PlayerHealth += healthAmount;
        }
    }
}
