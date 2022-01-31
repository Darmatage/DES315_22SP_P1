using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_Gem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player touches gem
        if (collision.gameObject.CompareTag("Player"))
        {
            //increment gem counter and update text
            DanielNunes_GemCounter gemCounter = FindObjectOfType<DanielNunes_GemCounter>();
            ++gemCounter.gemsFound;
            gemCounter.UpdateText();

            //destroy self
            Destroy(gameObject);
        }
    }
}
