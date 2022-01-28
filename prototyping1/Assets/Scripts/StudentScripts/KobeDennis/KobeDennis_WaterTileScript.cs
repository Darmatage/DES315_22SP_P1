using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KobeDennis_WaterTileScript : MonoBehaviour
{
    public SpriteRenderer tileSprite;
    public Sprite mixedSprite;
    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<KobeDennis_LavaBallScript>())
        {
            boxCollider.isTrigger = true;
            tileSprite.sprite = mixedSprite;
        }
    }
}
