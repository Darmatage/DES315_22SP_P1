using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimer : MonoBehaviour
{
    private float totalLifeTimer;
    private float lifeTimer;
    // Start is called before the first frame update
    void Start()
    {
        totalLifeTimer = 3.0f;
        lifeTimer = 0.0f;
    }

    void FixedUpdate()
    {
        lifeTimer += 0.1f;
        if (lifeTimer >= totalLifeTimer)
        {
            Destroy(gameObject);
        }
    }
}
