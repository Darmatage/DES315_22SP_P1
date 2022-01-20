using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantWu_SpinningSpikeDisk : MonoBehaviour
{
    public float y_dist;
    public float x_dist;
    public float y_center;
    public float x_center;

    public float speed;
    public bool reverse_speed;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (reverse_speed)
            transform.position = new Vector3(transform.position.x, y_center + -(Mathf.PingPong(Time.time * speed, y_dist) - y_dist / 2f), transform.position.z);
        else
            transform.position = new Vector3(transform.position.x, y_center + Mathf.PingPong(Time.time * speed, y_dist) - y_dist / 2f, transform.position.z);

        transform.Rotate(new Vector3(0, 0, 10000) * Time.deltaTime);
    }
}
