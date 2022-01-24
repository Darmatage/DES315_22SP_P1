using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDeletion : MonoBehaviour
{
    public float timer_ = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {

        timer_ -= Time.deltaTime;
        if (timer_ < 0)
        {
            DestroyObject(gameObject);
        }
    }
}
