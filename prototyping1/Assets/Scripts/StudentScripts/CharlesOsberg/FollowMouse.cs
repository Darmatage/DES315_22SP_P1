using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Camera cam = null;

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var cPos = transform.position;
        var mPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mPos.z = cPos.z;
        transform.position = mPos;
    }
}
