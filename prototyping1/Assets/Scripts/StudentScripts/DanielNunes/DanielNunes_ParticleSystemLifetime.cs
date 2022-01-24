using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_ParticleSystemLifetime : MonoBehaviour
{
    [SerializeField]
    private float deathTimer = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //destroy self after deathTimer is done
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
