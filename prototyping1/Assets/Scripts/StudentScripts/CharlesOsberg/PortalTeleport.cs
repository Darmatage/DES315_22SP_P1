using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PortalTeleport : MonoBehaviour
{
    private bool isActive = false;

    private const float pushAmt = 0.5f;
    private const float defaultForce = 25f;

    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Throw(Vector2 direction)
    {
        isActive = false;

        rigidbody.velocity = direction.normalized * defaultForce;
    }

    private void StopPortal()
    {
        rigidbody.velocity = Vector2.zero;

        isActive = true;
    }
    
    [SerializeField] private bool isPortal2 = false;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isActive)
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
            portalDist += (other.transform.position - transform.position).normalized * pushAmt;

            other.transform.position += portalDist;
        }
        else
        {
            //ensure collision is valid (i.e. not with player etc)
            var isCollisionValid = true;
            if (other.gameObject.CompareTag("Player"))
            {
                isCollisionValid = false;
            }

            if (isCollisionValid)
            {
                //determine direction of hit
                var hitVec = other.GetContact(0).point - new Vector2(transform.position.x, transform.position.y);
                if (Mathf.Abs(hitVec.y) > Mathf.Abs(hitVec.x))
                { //vertical hit
                    if (hitVec.y > 0)
                    { //hit on top
                        print("hit on top");
                     
                    }
                    else
                    { //hit on bottom
                        print("hit on bott");
                    }
                }
                else
                { //horizontal hit
                    if (hitVec.x > 0)
                    { //hit on right
                        print("hit on right");
                    }
                    else
                    { //hit on left
                        print("hit on left");
                    }
                }
                
                StopPortal();
            }

            
        }
    }
}
