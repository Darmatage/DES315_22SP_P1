using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_Cannonball : MonoBehaviour
{
    [SerializeField]
    private float speed;

    //singleton enforcement
    private static DanielNunes_Cannonball instance_;
    public static DanielNunes_Cannonball Instance
    {
        get
        {
            return instance_;
        }
    }
    private void Awake()
    {
        if (instance_ && instance_ != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance_ = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("TilemapWalls"))
        {
            Destroy(gameObject);
        }
    }
}
