using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreddyMartinMirrorShards : MonoBehaviour
{
    [HideInInspector]
    public float fadeTime;

    float fadeTimer = 0;

    void Update()
    {
        fadeTimer += Time.deltaTime;

        if (fadeTimer >= fadeTime)
        {
            Destroy(gameObject);
        }
    }
}
