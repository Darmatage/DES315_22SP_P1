using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantWu_SpinningSpikeDisk : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed;
    public bool reverse_speed;


    private float y_center;
    private float y_dist;
    // Start is called before the first frame update
    void Start()
    {
        y_dist = Mathf.Abs(pointA.y - pointB.y);
        y_center = Mathf.Ceil((pointA.y + pointB.y) / 2);
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
