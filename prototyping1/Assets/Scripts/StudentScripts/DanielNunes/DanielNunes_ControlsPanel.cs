using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_ControlsPanel : MonoBehaviour
{
    ////singleton enforcement
    //private static DanielNunes_ControlsPanel instance_;
    //public static DanielNunes_ControlsPanel Instance
    //{
    //    get
    //    {
    //        return instance_;
    //    }
    //}
    //private void Awake()
    //{
    //    if (instance_ && instance_ != this)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        instance_ = this;
    //    }
    //}

    //private Transform controlFire;
    //private Transform controlHold;
    //private Transform controlRotate;
    //private Transform controlAD;
    //private Transform controlDA;
    //private Transform controlWS;
    //private Transform controlSW;

    //private GameObject player;

    //private DanielNunes_Cannon[] allCannons;


    //private bool displayControlFire;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    //get the player
    //    player = GameObject.FindGameObjectWithTag("Player");

    //    //find all cannons in the scene
    //    allCannons = FindObjectsOfType<DanielNunes_Cannon>();

    //    controlFire = transform.Find("Control_Fire");
    //    controlHold = transform.Find("Control_Hold");
    //    controlRotate = transform.Find("Control_Rotate");
    //    controlAD = transform.Find("Control_AD");
    //    controlDA = transform.Find("Control_DA");
    //    controlWS = transform.Find("Control_WS");
    //    controlSW = transform.Find("Control_SW");
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    int i = 0;
    //    int j = 0;
    //    int k = 0;
    //    int l = 0;
    //    //constantly loop through every cannon in the scene
    //    foreach (DanielNunes_Cannon cannon in allCannons)
    //    {
    //        //if a cannon in the list is not usable
    //        if (!cannon.usable)
    //        {
    //            ++i;
    //        }

    //        //if a cannon in the list is not held onto
    //        if (!cannon.heldOnto)
    //        {
    //            ++j;
    //        }

    //        //if the player is not in range of a cannon in the list
    //        if (!cannon.inRange)
    //        {
    //            ++k;
    //        }

    //        //if player is not near a cannon
    //        if (cannon.whereIsPlayer == DanielNunes_Cannon.Where.eNOWHERE)
    //        {
    //            ++l;
    //        }
    //    }

    //    //if any cannon was usable
    //    if (i < allCannons.Length)
    //    {
    //        //display the fire controls
    //        controlFire.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        controlFire.gameObject.SetActive(false);
    //    }

    //    //if any cannon was held onto
    //    if (j < allCannons.Length)
    //    {
    //        controlFire.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        controlFire.gameObject.SetActive(false);
    //    }

    //    //if any cannon was in range
    //    if (k < allCannons.Length)
    //    {
    //        //display the hold controls
    //        controlHold.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        controlHold.gameObject.SetActive(false);
    //    }

    //    //if the player was near at least one cannon
    //    if (l < allCannons.Length)
    //    {

    //    }
    //}

    //private void PushingPullingControls()
    //{
    //    controlAD.gameObject.SetActive(false);
    //    controlDA.gameObject.SetActive(false);
    //    controlWS.gameObject.SetActive(false);
    //    controlSW.gameObject.SetActive(false);
    //}
}
