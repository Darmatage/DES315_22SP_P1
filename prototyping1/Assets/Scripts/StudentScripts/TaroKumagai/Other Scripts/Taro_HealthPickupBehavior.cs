using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taro_HealthPickupBehavior : MonoBehaviour
{
    public int healAmount = 30;
    private GameHandler gameHandlerObj;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
        if (gameHandlerLocation != null)
        {
            gameHandlerObj = gameHandlerLocation.GetComponent<GameHandler>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (GameHandler.PlayerHealth < gameHandlerObj.PlayerHealthStart)
            {
                gameHandlerObj.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
