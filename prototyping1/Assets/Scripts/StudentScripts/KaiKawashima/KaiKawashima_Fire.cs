using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaiKawashima_Fire : MonoBehaviour
{
    public int damage = 1;
    public bool onFire = false;
    private GameHandler gameHandler;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
        if (gameHandlerLocation != null)
        {
            gameHandler = gameHandlerLocation.GetComponent<GameHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onFire)
        { 
            if (collision.gameObject.CompareTag("Player"))
            {
                gameHandler.TakeDamage(damage);
            }
        }
    }
}
