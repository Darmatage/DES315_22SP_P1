using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JirakitJarusiripipat_Pickup : MonoBehaviour
{
    JirakitJarusiripipat_GameHandler gameHandler;

    public int healthAmount = 0;

    void Awake()
    {
        gameHandler = FindObjectOfType<JirakitJarusiripipat_GameHandler>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (JirakitJarusiripipat_GameHandler.PlayerHealth < gameHandler.PlayerHealthStart)
            {
                gameHandler.Heal(healthAmount);
                Destroy(gameObject);
            }
        }
    }
}
