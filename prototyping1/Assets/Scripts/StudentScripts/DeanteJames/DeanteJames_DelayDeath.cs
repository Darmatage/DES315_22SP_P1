using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeanteJames_DelayDeath : MonoBehaviour
{
    [SerializeField]
    public float secondsUntilDeath = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        secondsUntilDeath -= Time.deltaTime;

        if (secondsUntilDeath <= 0.0f)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
