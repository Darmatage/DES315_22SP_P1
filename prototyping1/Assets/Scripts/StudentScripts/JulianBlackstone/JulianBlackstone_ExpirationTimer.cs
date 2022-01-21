using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JulianBlackstone_ExpirationTimer : MonoBehaviour
{
    private float internalExpirationTime = 5.0f;

    public void SetExpirationTime(float newTime)
    {
        if (newTime < 0.0f)
        {
            newTime = 0.0f;
        }

        internalExpirationTime = newTime;
    }

    public void ForceExpire()
    {
        internalExpirationTime = 0.0f;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (internalExpirationTime > 0)
        {
            internalExpirationTime -= Time.deltaTime;
            return;
        }

        Destroy(gameObject);
    }
}
