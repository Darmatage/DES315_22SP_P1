using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaiKawashima_WaterBucket : MonoBehaviour
{
    public string keyToInteract = "e";
    [HideInInspector]
    public bool hasBucket = false;
    [HideInInspector]
    public bool hasWater = false;

    private bool inWaterRange = false;
    private bool inBucketRange = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inWaterRange)
        {
            if (Input.GetKeyDown(keyToInteract))
            {
                hasWater = true;
            }
        }
        else if (inBucketRange)
        {
            if (Input.GetKeyDown(keyToInteract))
            {
                hasBucket = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasBucket)
        {
            if (collision.gameObject.CompareTag("Water"))
            {
                inWaterRange = true;
                // display heads up
            }
        }
        else if (collision.gameObject.CompareTag("Bucket"))
        {
            inBucketRange = true;
            // display heads up
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBucket)
        {
            if (collision.gameObject.CompareTag("Water"))
            {
                inWaterRange = true;
                // display heads up
            }
        }
        else if (collision.gameObject.CompareTag("Bucket"))
        {
            inBucketRange = true;
            // display heads up
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            inWaterRange = false;
        }
        else if (collision.gameObject.CompareTag("Bucket"))
        {
            inBucketRange = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            inWaterRange = false;
        }
        else if (collision.gameObject.CompareTag("Bucket"))
        {
            inBucketRange = false;
        }

    }
}
