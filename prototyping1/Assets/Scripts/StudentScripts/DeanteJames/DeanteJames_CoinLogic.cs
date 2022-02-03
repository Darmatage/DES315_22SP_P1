using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeanteJames_CoinLogic : MonoBehaviour
{
    public static bool textDisplayed = false;
    public GameObject goldInstruction;
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
            timer.gameObject.GetComponent<DeanteJames_TimerLogic>().AddDeduction(10.0f);
            GameObject.Destroy(gameObject);

            if (textDisplayed == false)
            {
                textDisplayed = true;
                GameObject pic = GameObject.Instantiate(goldInstruction, collision.transform.position, Quaternion.identity);
                timer.GetComponent<DeanteJames_TimerLogic>().pauseGame(pic);
            }
        }
    }
}
