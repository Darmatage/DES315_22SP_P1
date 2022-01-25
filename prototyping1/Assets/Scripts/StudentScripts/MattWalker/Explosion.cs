using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float ExplosionTime = 0.25f;

    private float LifeTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Animation>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        LifeTimer += Time.deltaTime;
        if (LifeTimer >= ExplosionTime)
            Destroy(gameObject);
    }
}
