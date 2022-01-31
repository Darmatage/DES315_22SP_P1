using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarryChen_FogOfWar_HighGroundScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}