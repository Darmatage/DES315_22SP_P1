using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JulianBlackstone_ColorSystem : MonoBehaviour
{
    public string colorTag = "Red";
    public bool hideOnActivation = false;

    public GameObject ItemOutline;
    public bool ShowOutlineOfObject = false;

    private float internalTimer = 0.0f;
    private SpriteRenderer mySprite;

    private void Start()
    {
        if (ItemOutline != null)
        {
            ItemOutline.SetActive(ShowOutlineOfObject);
        }


        mySprite = GetComponent<SpriteRenderer>();

        if (mySprite == null)
        {
            Debug.LogError("Sprite was not found for object:" + gameObject.name);
        }

        if (hideOnActivation == false)
        {
            GetComponent<BoxCollider2D>().enabled = false;

            Color instanceColor = mySprite.color;

            instanceColor.a = 0;

            mySprite.color = instanceColor;

            //GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void ActivateColorEffect(float xSeconds)
    {
        // we add a second to the timer to detect finished state inversion more easily
        xSeconds += 1.0f;
        if (hideOnActivation) Hide(xSeconds);
        if (!hideOnActivation) Reveal(xSeconds);
    }

    private void FixedUpdate()
    {

       
        if (mySprite != null)
        {
            if (internalTimer >= 1.0f)
            {
                Color instanceColor = mySprite.color;

                if (hideOnActivation)
                {
                    instanceColor.a = 1.0f / internalTimer;
                }
                else
                {
                    instanceColor.a = internalTimer / 2.0f;
                }

                mySprite.color = instanceColor;
            }
        }

        if (internalTimer >= 1.0f)
        {
            internalTimer -= Time.deltaTime;
            return;
        }

        if ((internalTimer > 0.0f) && (internalTimer < 1.0f))
        {
            internalTimer = 0.0f;
            GetComponent<BoxCollider2D>().enabled = !(GetComponent<Collider2D>().enabled);
            //GetComponent<SpriteRenderer>().enabled = !(GetComponent<SpriteRenderer>().enabled);

            Color instanceColor = mySprite.color;


            if (hideOnActivation)
            {
                instanceColor.a = 1;
            }
            else
            {
                instanceColor.a = 0;
            }

            mySprite.color = instanceColor;
        }



    }

    private void Reveal(float xSeconds)
    {
        internalTimer = xSeconds;
        GetComponent<BoxCollider2D>().enabled = true; 
       // GetComponent<SpriteRenderer>().enabled = true;
    }


    public void Hide(float xSeconds)
    {
        internalTimer = xSeconds;
        GetComponent<BoxCollider2D>().enabled = false;
        //GetComponent<SpriteRenderer>().enabled = false;
    }

}
