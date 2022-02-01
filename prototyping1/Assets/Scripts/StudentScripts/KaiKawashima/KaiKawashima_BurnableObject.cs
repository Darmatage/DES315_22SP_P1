using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaiKawashima_BurnableObject : MonoBehaviour
{
    public GameObject objectToBurn;
    public string keyToInteract = "e";
    public SpriteRenderer sprite;
    [HideInInspector]
    public bool extinguished = false;

    private bool onFire;
    private GameObject player = null;
    private KaiKawashima_WaterBucket waterBucket;
    private bool withinRange = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            waterBucket = player.GetComponent<KaiKawashima_WaterBucket>();
        }
        onFire = objectToBurn.GetComponent<KaiKawashima_Fire>().onFire;
    }

    // Update is called once per frame
    void Update()
    {
        if (withinRange)
        {
            if (Input.GetKeyDown(keyToInteract))
            {
                // extinguish fire and make it passable for player
                onFire = false;
                extinguished = true;
                objectToBurn.GetComponent<BoxCollider2D>().isTrigger = true;
                sprite.enabled = false;
                waterBucket.UseWater();
                waterBucket.DeactivateHUD();
                waterBucket.imageIndicator.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (onFire)
            {
                // check if the player has water
                if (waterBucket.hasWater)
                {
                    withinRange = true;
                    // display message
                    waterBucket.imageIndicator.SetActive(true);
                }
                waterBucket.ActivateHUD();
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        { 
            withinRange = false;
            waterBucket.DeactivateHUD();
            waterBucket.imageIndicator.SetActive(false);
        }
    }
}
