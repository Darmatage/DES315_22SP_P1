using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeanteJames_JewelLogic : MonoBehaviour
{
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
            GameObject timer = GameObject.Find("Timer");
            timer.gameObject.GetComponent<DeanteJames_TimerLogic>().FreezeTime(true);
            GameObject.Destroy(gameObject);
        }
    }
}
