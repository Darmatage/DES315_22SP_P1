using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    [SerializeField] private Transform plyTransform = null;

    [SerializeField] private GameObject PortalableLayer = null;

    [SerializeField] private GameObject portalObject1 = null;
    [SerializeField] private GameObject portalObject2 = null;

    public static GameObject curPor1 { get; private set; } = null;
    public static GameObject curPor2 { get; private set; } = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var wPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            wPos.z = -0.1f;

            if (!curPor1)
            {
                curPor1 = Instantiate(portalObject1, wPos, Quaternion.identity);
            }
            else
            {
                curPor1.transform.position = wPos;
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            var wPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            wPos.z = -0.1f;

            if (!curPor2)
            {
                curPor2 = Instantiate(portalObject2, wPos, Quaternion.identity);
            }
            else
            {
                curPor2.transform.position = wPos;
            }        
        }
    }
}
