using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JirakitJarusiripipat_DestroyByTime : MonoBehaviour
{
    public float timer = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
