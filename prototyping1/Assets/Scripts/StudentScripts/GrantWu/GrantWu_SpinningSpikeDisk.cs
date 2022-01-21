using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantWu_SpinningSpikeDisk : MonoBehaviour
{
    public float y_dist;       // length of disk to travel in the y direction
    public float x_dist;       // length of disk to travel in the x direction
    public float y_center;     // starting position of y coordinate used to offset PingPong()
    public float x_center;     // starting position of x coordinate used to offset PingPong()
    public float speed = 2;    // speed of disk
    public bool reverse_speed; // reverse direction of disk
    public int damage = 1;     // player damage of disk 
    public bool y_dir;
    public bool x_dir;


    private GameHandler gameHandlerObj; // player takes damage

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
        if (gameHandlerLocation != null)
        {
            gameHandlerObj = gameHandlerLocation.GetComponent<GameHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Mathf.PingPong returns an incrementing and decrementing value between 0 and length (distance here).
        // Center position is used and offsetted by PingPong's return value
        // Subtract half the length so that it moves +/- length/2 e.g. length = 7 -> moves 3.5 units up and 3.5 down
        if (y_dir)
        {
            if (reverse_speed)
                transform.position = new Vector3(transform.position.x, y_center + -(Mathf.PingPong(Time.time * speed, y_dist) - y_dist / 2f), transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, y_center + Mathf.PingPong(Time.time * speed, y_dist) - y_dist / 2f, transform.position.z);
        }
        else if (x_dir)
        {
            if (reverse_speed)
                transform.position = new Vector3(x_center + -(Mathf.PingPong(Time.time * speed, x_dist) - x_dist / 2f), transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(x_center + (Mathf.PingPong(Time.time * speed, x_dist) - x_dist / 2f), transform.position.y, transform.position.z);
        }

        transform.Rotate(new Vector3(0, 0, 10000) * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    { 
        if (collision.gameObject.tag == "Player")
        {
            gameHandlerObj.TakeDamage(damage);
        }
    }
}
