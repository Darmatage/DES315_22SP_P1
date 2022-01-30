using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint LastCheckpoint { get; private set; }

    public GameObject SwitchOffArt;
    public GameObject SwitchOnArt;
    //private bool isActive;
    public GameObject PlayerObj;
    // Start is called before the first frame update
    void Start()
    {
        SwitchOffArt.SetActive(true);
        SwitchOnArt.SetActive(false);
        //isActive = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            SwitchOffArt.SetActive(false);
            SwitchOnArt.SetActive(true);
            if (LastCheckpoint != this)
            {
                if (LastCheckpoint)
                {
                    LastCheckpoint.SwitchOffArt.SetActive(true);
                    LastCheckpoint.SwitchOnArt.SetActive(false);
                }
                LastCheckpoint = this;
            }
        }
    }

    private void Update()
    {
        if(GameHandler.PlayerHealth <= 2)
        {
            GameHandler.PlayerHealth = 100;
            PlayerObj.transform.position = LastCheckpoint.gameObject.transform.position;
        }
    }

}
