using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenThompson_Heart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject handler = GameObject.FindGameObjectWithTag("GameHandler");
            GameHandler gh = handler.GetComponent<GameHandler>();
            gh.Heal(10);
            Destroy(gameObject);
        }
    }
}
