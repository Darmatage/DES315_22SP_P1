using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerOnPickup : MonoBehaviour
{
    public int HealAmount = 25;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameHandler gh = FindObjectOfType<GameHandler>();

            if (gh != null)
                gh.Heal(HealAmount);

            Destroy(gameObject);
        }
    }
}
