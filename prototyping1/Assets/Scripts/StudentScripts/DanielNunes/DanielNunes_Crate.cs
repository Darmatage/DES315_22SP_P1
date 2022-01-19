using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_Crate : MonoBehaviour
{
    [SerializeField]
    private Sprite shatter;

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
        if (collision.gameObject.CompareTag("DN_Cannonball"))
        {
            GetComponent<SpriteRenderer>().sprite = shatter;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
