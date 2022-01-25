using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject SwitchOffArt;
    public GameObject SwitchOnArt;
    //private bool isActive;
    public GameObject CheckpointObj;
    public GameObject Handler;
    public GameObject PlayerObj;
    // Start is called before the first frame update
    void Start()
    {
        SwitchOffArt.SetActive(true);
        SwitchOnArt.SetActive(false);
        //isActive = false;
        GameHandler.PlayerHealth = 5;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            SwitchOffArt.SetActive(false);
            SwitchOnArt.SetActive(true);
            //isActive = true;
            CheckpointObj.GetComponent<Checkpoint>();
        }
    }

    private void Update()
    {
        if(GameHandler.PlayerHealth < 2)
        {
            GameHandler.PlayerHealth = 5;
            PlayerObj.transform.position = new Vector3(4.6f, -4.2f, 0.0f);
        }
    }

}
