using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KobeDennis_LavaTileScript : MonoBehaviour
{

    public float lifeTime = 0f;
    public SpriteRenderer spriteRenderer;
    public Color aphlaColor;
    private float halfLifeTime;
    // Start is called before the first frame update
    void Start()
    {
        halfLifeTime = lifeTime / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if(lifeTime <= halfLifeTime)
        {
            spriteRenderer.color = aphlaColor;
        }

        if(lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
