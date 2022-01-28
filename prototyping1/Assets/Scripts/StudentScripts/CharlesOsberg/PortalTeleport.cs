using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PortalTeleport : MonoBehaviour
{
    private bool isActive = false;

    private const float pushAmt = 0.5f;
    private const float defaultForce = 25f;

    private const float portalExclusionZone = 1.0f;


    [SerializeField] private GameObject portalFizz = null;
    
    private new Rigidbody2D rigidbody;

    public enum HitDirection
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public HitDirection LastDir { get; private set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Throw(Vector2 direction)
    {
        rigidbody.isKinematic = false;
        isActive = false;
        rigidbody.velocity = direction.normalized * defaultForce;
        transform.position += new Vector3(direction.x, direction.y, 0).normalized * pushAmt * 2.5f;
    }

    private void StopPortal()
    {
        var rawPos = transform.position;

        switch (LastDir)
        {
            case HitDirection.Bottom:
                rawPos.y = Mathf.Floor(rawPos.y);
                break;
            case HitDirection.Top:
                rawPos.y = Mathf.Ceil(rawPos.y);
                break;
            case HitDirection.Left:
                rawPos.x = Mathf.Floor(rawPos.x);
                break;
            case HitDirection.Right:
                rawPos.x = Mathf.Ceil(rawPos.x);
                break;
        }
        
        transform.position = rawPos;
        PortalGun.NotifyPortalSuccess(isPortal2);
        rigidbody.isKinematic = true;
        isActive = true;
        rigidbody.velocity = Vector2.zero;
    }
    
    [SerializeField] private bool isPortal2 = false;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isActive)
        {
            HitDirection oDir = HitDirection.Bottom;
            var isValidTele = false;
            var portalDist = new Vector3();
            if (isPortal2)
            {
                if (PortalGun.curPor1)
                {
                    oDir = PortalGun.curPor1.GetComponent<PortalTeleport>().LastDir;
                    portalDist = PortalGun.curPor1.transform.position - transform.position;
                    isValidTele = true;
                }
            }
            else
            {
                if (PortalGun.curPor2)
                {
                    oDir = PortalGun.curPor2.GetComponent<PortalTeleport>().LastDir;
                    portalDist = PortalGun.curPor2.transform.position - transform.position;
                    isValidTele = true;
                }
            }
            
            //some exclusionary checks
            if (!COTags.CompareTag(other.gameObject, "CanGoThroughPortals"))
            {
                isValidTele = false;
            }

            if (isValidTele)
            {
                //push away a bit so we don't immediately teleport back and forth
                portalDist -= (other.transform.position - transform.position);

                //also push away from contacted wall
                portalDist += oDir switch
                {
                    HitDirection.Bottom => Vector3.up * 2.0f,
                    HitDirection.Top => Vector3.down,
                    HitDirection.Left => Vector3.right,
                    HitDirection.Right => Vector3.left,
                    _ => Vector3.zero
                };
                
                other.transform.position += portalDist;
            }

        }
        else
        {
            //ensure collision is valid (i.e. not with player etc)
            var isCollisionValid = true;
            //for feedback
            var shouldFizzle = false;
            
            //collision with player and other goodies not valid
            if (COTags.CompareTag(other.gameObject, "PortalPassThru"))
            {
                isCollisionValid = false;
            }
            else if (!COTags.CompareTag(other.gameObject, "PortalAllowed"))
            {
                shouldFizzle = true;
                isCollisionValid = false;
            }
            else if (isPortal2)
            { //too close to other portal (makes it possible to glitch out of bounds)
                if (PortalGun.curPor1)
                {
                    if (Vector3.Distance(
                        PortalGun.curPor1.transform.position,
                        transform.position)
                            < portalExclusionZone)
                    {
                        isCollisionValid = false;
                        shouldFizzle = true;
                    }
                }
            }
            else
            {
                if (PortalGun.curPor2)
                {
                    if (Vector3.Distance(
                            PortalGun.curPor2.transform.position,
                            transform.position)
                        < portalExclusionZone)
                    {
                        isCollisionValid = false;
                        shouldFizzle = true;
                    }
                }
            }

            if (isCollisionValid)
            {
                //determine direction of hit
                var hitVec = other.GetContact(0).point - new Vector2(transform.position.x, transform.position.y);
                if (Mathf.Abs(hitVec.y) > Mathf.Abs(hitVec.x))
                { //vertical hit
                    if (hitVec.y > 0)
                    { //hit on top
                        LastDir = HitDirection.Top;
                    }
                    else
                    { //hit on bottom
                        LastDir = HitDirection.Bottom;
                    }
                }
                else
                { //horizontal hit
                    if (hitVec.x > 0)
                    { //hit on right
                        LastDir = HitDirection.Right;
                    }
                    else
                    { //hit on left
                        LastDir = HitDirection.Left;
                    }
                }
                
                StopPortal();
            }

            if (shouldFizzle)
            {
                Instantiate(portalFizz, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
