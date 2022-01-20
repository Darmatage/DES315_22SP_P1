using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_CannonTrigger : MonoBehaviour
{
    private DanielNunes_Cannon cannon;

    // Start is called before the first frame update
    void Start()
    {
        cannon = transform.parent.GetComponent<DanielNunes_Cannon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player is touching the backside of the cannon
        if (collision.gameObject.CompareTag("Player"))
        {
            //we can fire it
            cannon.usable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if player is no longer touching the backside of the cannon
        if (collision.gameObject.CompareTag("Player"))
        {
            //we cannot fire it
            cannon.usable = false;
        }
    }
}
