using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeanteJames_JewelLogic : MonoBehaviour
{
    public static bool textDisplayed = false;
    public GameObject diamondInstruction;
    public AudioClip clip;

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

            collision.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
            if (textDisplayed == false)
            {
                textDisplayed = true;
                GameObject pic = GameObject.Instantiate(diamondInstruction, collision.transform.position, Quaternion.identity);
                timer.GetComponent<DeanteJames_TimerLogic>().pauseGame(pic);
            }
        }
    }
}
