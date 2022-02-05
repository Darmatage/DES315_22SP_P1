using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeanteJames_TimerTutorial : MonoBehaviour
{
    public GameObject TimerInstruction;

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
            GameObject pic = GameObject.Instantiate(TimerInstruction, collision.transform.position, Quaternion.identity);
            timer.GetComponent<DeanteJames_TimerLogic>().pauseGame(pic);
        }

        GameObject.Destroy(gameObject);
    }
}
