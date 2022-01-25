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
        rigidbody.isKinematic = false;
        isActive = false;
        rigidbody.velocity = direction.normalized * defaultForce;
        transform.position += new Vector3(direction.x, direction.y, 0).normalized * pushAmt * 2.0f;
    }

    private void StopPortal()
    {
        var rawPos = transform.position;
        rawPos.x = Mathf.Floor(rawPos.x);
        rawPos.y = Mathf.Floor(rawPos.y);
        transform.position = rawPos;
        
        rigidbody.isKinematic = true;
        isActive = true;
        rigidbody.velocity = Vector2.zero;
    }
    
    [SerializeField] private bool isPortal2 = false;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isActive)
        {
            var isValidTele = false;
            var portalDist = new Vector3();
            if (isPortal2)
            {
                if (PortalGun.curPor1)
                {
                    portalDist = PortalGun.curPor1.transform.position - transform.position;
                    isValidTele = true;
                }
            }
            else
            {
                if (PortalGun.curPor2)
                {
                    portalDist = PortalGun.curPor2.transform.position - transform.position;
                    isValidTele = true;
                }
            }

            if (isValidTele)
            {
                //push away a bit so we don't immediately teleport back and forth
                portalDist += (other.transform.position - transform.position).normalized * pushAmt;

                other.transform.position += portalDist;
            }

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
                        
                    }
                    else
                    { //hit on bottom
                        
                    }
                }
                else
                { //horizontal hit
                    if (hitVec.x > 0)
                    { //hit on right
                        
                    }
                    else
                    { //hit on left
                        
                    }
                }
                
                StopPortal();
            }

            
        }
    }
}
