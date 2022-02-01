using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeanteJames_HealthLogic : MonoBehaviour
{
    public int HealthRegain = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject timer = GameObject.Find("GameHandler");
            timer.gameObject.GetComponent<JirakitJarusiripipat_GameHandler>().Heal(HealthRegain);
            GameObject.Destroy(gameObject);
        }
    }
}
