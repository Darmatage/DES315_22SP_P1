using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JulianBlackstone_ColorSystem : MonoBehaviour
{
    public string colorTag = "Red";
    public bool hideOnActivation = false;

    private float internalTimer = 0.0f;


    private void Start()
    {
        if (hideOnActivation == false)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void ActivateColorEffect(float xSeconds)
    {
        // <- we add a second to the timer to detect finished state inversion more easily
        xSeconds += 1.0f;
        if (hideOnActivation) Hide(xSeconds);
        if (!hideOnActivation) Reveal(xSeconds);
    }

    private void FixedUpdate()
    {
        if (internalTimer >= 1.0f)
        {
            internalTimer -= Time.deltaTime;
            return;
        }

        if ((internalTimer > 0.0f) && (internalTimer < 1.0f))
        {
            internalTimer = 0.0f;
            //GetComponent<Collider2D>().enabled = !(GetComponent<Collider2D>().enabled); // change this later
            GetComponent<BoxCollider2D>().enabled = !(GetComponent<Collider2D>().enabled);
            GetComponent<SpriteRenderer>().enabled = !(GetComponent<SpriteRenderer>().enabled);

        }


    }

    private void Reveal(float xSeconds)
    {
        internalTimer = xSeconds;
        GetComponent<BoxCollider2D>().enabled = true; 
        GetComponent<SpriteRenderer>().enabled = true;
    }


    public void Hide(float xSeconds)
    {
        internalTimer = xSeconds;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

}
