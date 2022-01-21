using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JulianBlackstone_ColorWaveScript : MonoBehaviour
{
    public string colorTag = "Red";
    private float expansionVelocity = 1.0f;
    private float objectRevealTime = 5.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newScale = transform.localScale;

        float scaleFactor = expansionVelocity * Time.deltaTime;
        newScale.x += scaleFactor;
        newScale.y += scaleFactor;

        transform.localScale = newScale;

    }

    public void SetExpansionVelocity(float newVel)
    {
        if (newVel <= 0.0f)
        {
            Debug.LogError("A wave was provided a non-positive expansion velocity");
            return;
        }

        expansionVelocity = newVel;
    }

    public void SetRevealTime(float newTime)
    {
        if (newTime <= 0.0f)
        {
            Debug.LogError("A wave was provided a non-positive reveal time value");
            return;
        }

        objectRevealTime = newTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherObj = collision.gameObject;

        JulianBlackstone_ColorSystem colorSys = otherObj.GetComponent<JulianBlackstone_ColorSystem>();

        if (colorSys == null) return;

        if (colorSys.colorTag == colorTag)
        {
            colorSys.ActivateColorEffect(objectRevealTime);
        }
        

    }

   
}
