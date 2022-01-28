using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_Cannonball : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float deathTimer = 15.0f;

    [SerializeField]
    private GameObject cannonballParticles;
    [SerializeField]
    private GameObject cannonParticles;

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

        //we don't want to be a child of the cannon we were instantiated from anymore
        transform.parent = null;

        //create cannon particles
        //cannonball creates these particles so you can't constantly create them when spamming shoot on the cannon itself
        GameObject p = Instantiate(cannonParticles, null);
        p.transform.position = transform.position;
        //match the angle of the particles with the cannonball (when cannonball is at z = 0, particles are at z = -90)
        p.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 90.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //if, for whatever reason, someone has an insanely large level where the cannonball can travel for a while, have this death timer
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0.0f)
        {
            CreateParticles();
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
            CreateParticles();
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

            CreateParticles();

            //despawn
            Destroy(gameObject);
        }
        //if we collided with one of Taro's switches
        else if (collision.gameObject.GetComponent<Taro_ColorSwitchBehavior>())
        {
            //get the color manager from the player
            Taro_ColorSwitchTrigger trigger = GameObject.FindGameObjectWithTag("Player").GetComponent<Taro_ColorSwitchTrigger>();
            if (trigger != null && trigger.isActive)
            {
                //get the color of the switch we hit
                Taro_ColorSwitchBehavior taroSwitch = collision.gameObject.GetComponent<Taro_ColorSwitchBehavior>();

                Taro_ColorSwitchManager.SetActiveColor(taroSwitch.SwitchColor);

                taroSwitch.ActiveSprite.SetActive(true);
                taroSwitch.InactiveSprite.SetActive(false);

                //get all cannons in the scene
                DanielNunes_Cannon[] cannons = FindObjectsOfType<DanielNunes_Cannon>();
                //go through all cannons and...
                for (int i = 0; i < cannons.Length; ++i)
                {
                    //...briefly reset each of their raycast contacts
                    cannons[i].ResetContacts();
                }

                //despawn cannonball
                CreateParticles();
                Destroy(gameObject);
            }
        }
        //if we collided with one of Taro's tilemap blocks while it is on the default collision layer
        else if (collision.gameObject.name.Contains("Taro_Tilemap") && collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            //despawn cannonball
            CreateParticles();
            Destroy(gameObject);
        }
    }

    public void CreateParticles()
    {
        GameObject particles = Instantiate(cannonballParticles, null);
        particles.transform.position = transform.position;
    }
}
