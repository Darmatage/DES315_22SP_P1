using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantWu_SpinningSpikeDisk : MonoBehaviour
{
    [Tooltip("move disk on y axis")]
    public bool y_dir;         // move disk on y axis
    [Tooltip("move disk on x axis")]
    public bool x_dir;         // move disk on x axis
    [Tooltip("reverse direction of disk")]
    public bool reverse_dir; // reverse direction of disk
    [Tooltip("change diagonal direction from bottom left/top right to bottom right/top left")]
    public bool reverse_diagonal_dir; // change diagonal direction from bottom left/top right to bottom right/top left
    [Tooltip("length of disk to travel in the y direction")]
    public float y_dist;       // length of disk to travel in the y direction
    [Tooltip("length of disk to travel in the x direction")]
    public float x_dist;       // length of disk to travel in the x direction
    [Tooltip("starting position of y direction")]
    public float y_center;     // starting position of y coordinate used to offset PingPong()
    [Tooltip("starting position of x direction")]
    public float x_center;     // starting position of x coordinate used to offset PingPong()
    [Tooltip("speed of disk")]
    public float speed = 2;    // speed of disk
    [Tooltip("move disk in a circle")]
    public bool circle_mode;   // move the disk in a circle
    [Tooltip("speed of rotating disk")]
    public float rotate_speed = 1;
    [Tooltip("distance from disk position to rotate about")]
    public float radius = 0.5f;
    [Tooltip("player damage from disk")]
    public int damage = 1;     // player damage of disk 
   
   
    private GameHandler gameHandlerObj; // player takes damage
    private Vector2 center;             // object's pivot point/initial starting position
    private float angle;                // uses rotate speed and updates with delta time for sin & cos angles
    AudioSource audio;                  // player hurt audio clip
    SpriteRenderer sprite;              // disk sprite

    // Start is called before the first frame update
    void Start()
    {
        center = transform.position; // set pivot point
        audio = GetComponent<AudioSource>(); // grab hurt audio for player damage
        sprite = GetComponentInChildren<SpriteRenderer>();

        // Game handler to handle player damage
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
        #region Linear Movement
        if (x_dir && y_dir)
        {
            if (reverse_diagonal_dir)
            {
                if (reverse_dir)
                    transform.position = new Vector3(x_center + (Mathf.PingPong(Time.time * speed, x_dist) - x_dist / 2f), y_center + -(Mathf.PingPong(Time.time * speed, y_dist) - y_dist / 2f), transform.position.z);
                else
                    transform.position = new Vector3(x_center + -(Mathf.PingPong(Time.time * speed, x_dist) - x_dist / 2f), y_center + Mathf.PingPong(Time.time * speed, y_dist) - y_dist / 2f, transform.position.z);
            }
            else
            {
                if (reverse_dir)
                    transform.position = new Vector3(x_center + -(Mathf.PingPong(Time.time * speed, x_dist) - x_dist / 2f), y_center + -(Mathf.PingPong(Time.time * speed, y_dist) - y_dist / 2f), transform.position.z);
                else
                    transform.position = new Vector3(x_center + (Mathf.PingPong(Time.time * speed, x_dist) - x_dist / 2f), y_center + Mathf.PingPong(Time.time * speed, y_dist) - y_dist / 2f, transform.position.z);
            }
        }
        else if (y_dir)
        {
            if (reverse_dir)
                transform.position = new Vector3(transform.position.x, y_center + -(Mathf.PingPong(Time.time * speed, y_dist) - y_dist / 2f), transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, y_center + Mathf.PingPong(Time.time * speed, y_dist) - y_dist / 2f, transform.position.z);
        }
        else if (x_dir)
        {
            if (reverse_dir)
                transform.position = new Vector3(x_center + -(Mathf.PingPong(Time.time * speed, x_dist) - x_dist / 2f), transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(x_center + (Mathf.PingPong(Time.time * speed, x_dist) - x_dist / 2f), transform.position.y, transform.position.z);
        }
        #endregion
        #region Circle Movement
        else if (circle_mode)
        {
            if (reverse_dir) // counter clockwise
                angle += -rotate_speed * Time.deltaTime; // rotate smoothly with delta time
            else // clockwise
                angle += rotate_speed * Time.deltaTime; // rotate smoothly with delta time
            var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius; // circle (sin, cos) * radius length
            transform.position = center + offset; // offset from center acting as pivot point
        }
        #endregion
        else // should never happen 
        {
            Debug.LogError("Invalid mode combination for disk movement. Circle_mode does not work with linear directions.");
        }

        transform.Rotate(new Vector3(0, 0, 10000) * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameHandlerObj.TakeDamage(damage);
            audio.Play();
            sprite.color = new Color(1, 0, 0);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject, 0.2f);
            audio.Play();
            sprite.color = new Color(1, 0, 0);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            sprite.color = new Color(1, 1, 1, 1);
        if (collision.gameObject.tag == "Enemy")
            sprite.color = new Color(1, 1, 1, 1);
    }
}
