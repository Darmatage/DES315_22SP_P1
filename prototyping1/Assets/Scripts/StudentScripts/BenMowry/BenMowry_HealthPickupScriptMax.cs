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
        if (col.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            gameHandler.Heal(gameHandler.PlayerHealthStart);
        }
    }
}
