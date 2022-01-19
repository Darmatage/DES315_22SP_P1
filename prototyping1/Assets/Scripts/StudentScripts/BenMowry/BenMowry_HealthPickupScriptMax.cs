using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenMowry_HealthPickupScriptMax : MonoBehaviour
{
    GameHandler gameHandler;

    public int healthAmount = 0;

    void Awake()
    {
        gameHandler = FindObjectOfType<GameHandler>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
        gameHandler.Heal(gameHandler.PlayerHealthStart);
    }
}
