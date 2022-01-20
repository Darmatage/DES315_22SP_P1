using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_Cannonball : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float deathTimer = 15.0f;

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
        //Because the cannonball is instantiated from the cannon, it is a child of said cannon.
        //We also want the cannonball to move in the direction the cannon was facing.
        //So, use the right vector of the cannon in this calculation.
        GetComponent<Rigidbody2D>().velocity = transform.parent.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        //if, for whatever reason, someone has an insanely large level where the cannonball can travel for a while, have this death timer
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    //get the speed of the cannonball
    public float GetSpeed()
    {
        return speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the cannonball hits any walls
        if (collision.gameObject.name.Equals("TilemapWalls"))
        {
            //despawn
            Destroy(gameObject);
        }
        //if the cannonball hits the switch
        else if (collision.gameObject.name.Equals("Switch"))
        {
            //change the sprite and unlock the door
            DoorSwitch ds = collision.GetComponent<DoorSwitch>();
            ds.SwitchOffArt.SetActive(false);
            ds.SwitchOnArt.SetActive(true);
            ds.DoorObj.GetComponent<Door>().DoorOpen();

            //despawn
            Destroy(gameObject);
        }
    }
}
