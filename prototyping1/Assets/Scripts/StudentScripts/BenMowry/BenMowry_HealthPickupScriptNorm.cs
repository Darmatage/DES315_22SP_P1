using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenMowry_HealthPickupScriptNorm : MonoBehaviour
{
    GameHandler gameHandler;

    public int healthAmount = 0;

    void Awake()
    {
        gameHandler = FindObjectOfType<GameHandler>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(GameHandler.PlayerHealth < gameHandler.PlayerHealthStart)
        {
            Destroy(gameObject);
            gameHandler.Heal(healthAmount);
        }
    }
}
