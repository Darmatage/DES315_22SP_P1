using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaiKawashima_WaterBucket : MonoBehaviour
{
    public string keyToInteract = "e";
    public GameObject canvas;
    public GameObject imageBucketEmpty;
    public GameObject imageBucketFull;
    public Vector3 bucketUILocation;
    public GameObject keyHUD;
    public Sprite imageBucketEmptyWithIndicator;
    [HideInInspector]
    public bool hasBucket = false;
    [HideInInspector]
    public bool hasWater = false;

    private GameObject refBucket;
    private Image imageBucketEmptyObject;
    private bool inWaterRange = false;
    private bool inBucketRange = false;
    

    // Start is called before the first frame update
    void Start()
    {
        imageBucketFull = Instantiate(imageBucketFull, bucketUILocation, Quaternion.identity, canvas.transform);
        imageBucketFull.SetActive(false);

        imageBucketEmpty = Instantiate(imageBucketEmpty, bucketUILocation, Quaternion.identity, canvas.transform);
        imageBucketEmpty.SetActive(false);
        imageBucketEmptyObject = imageBucketEmpty.GetComponentInChildren<Image>();

        Vector3 hudPosition;
        hudPosition.x = Screen.width / 2;
        hudPosition.y = 3 * Screen.height / 4;
        hudPosition.z = 0;
        
        keyHUD = Instantiate(keyHUD, hudPosition, Quaternion.identity, canvas.transform);
        Text hudText = keyHUD.GetComponentInChildren<Text>();
        hudText.text = keyToInteract.ToUpper();
        keyHUD.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inWaterRange)
        {
            if (Input.GetKeyDown(keyToInteract))
            {
                hasWater = true;
                //SwapSprites();
                imageBucketEmpty.SetActive(false);
                imageBucketFull.SetActive(true);
                DeactivateHUD();
            }
        }
        else if (inBucketRange)
        {
            if (Input.GetKeyDown(keyToInteract))
            {
                hasBucket = true;
                Destroy(refBucket);
                imageBucketEmpty.SetActive(true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasBucket)
        {
            if (collision.gameObject.CompareTag("Water") && !hasWater)
            {
                inWaterRange = true;
                // display heads up
                keyHUD.SetActive(true);
                SwapSprites();
            }
        }
        else if (collision.gameObject.CompareTag("Bucket"))
        {
            inBucketRange = true;
            refBucket = collision.gameObject;
            // display heads up
            keyHUD.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBucket)
        {
            if (collision.gameObject.CompareTag("Water") && !hasWater)
            {
                inWaterRange = true;
                // display heads up
                keyHUD.SetActive(true);
                SwapSprites();
            }
        }
        else if (collision.gameObject.CompareTag("Bucket"))
        {
            inBucketRange = true;
            refBucket = collision.gameObject;
            // display heads up
            keyHUD.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            inWaterRange = false;
            keyHUD.SetActive(false);
            SwapSprites();
        }
        else if (collision.gameObject.CompareTag("Bucket"))
        {
            inBucketRange = false;
            keyHUD.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            inWaterRange = false;
            keyHUD.SetActive(false);
            SwapSprites();
        }
        else if (collision.gameObject.CompareTag("Bucket"))
        {
            inBucketRange = false;
            keyHUD.SetActive(false);
        }

    }

    private void SwapSprites()
    {
        Sprite temp = imageBucketEmptyObject.sprite;
        imageBucketEmptyObject.sprite = imageBucketEmptyWithIndicator;
        imageBucketEmptyWithIndicator = temp;
    }

    public void UseWater()
    {
        if (hasWater)
        {
            hasWater = false;
            imageBucketFull.SetActive(false);
            imageBucketEmpty.SetActive(true);
        }
    }

    public void ActivateHUD()
    {
        keyHUD.SetActive(true);
    }

    public void DeactivateHUD()
    {
        keyHUD.SetActive(false);
    }
}
