using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    [SerializeField] private bool isPortal2 = false;
    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector3 portalDist;
        if (isPortal2)
        {
            portalDist = PortalGun.curPor1.transform.position - transform.position;
        }
        else
        {
            portalDist = PortalGun.curPor2.transform.position - transform.position ;
        }
        
        //push away a bit so we don't immediately teleport back and forth
        portalDist += other.transform.position - transform.position;

        other.transform.position += portalDist;
    }
}
