//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EunjinHong_DashMove : MonoBehaviour
//{
//    private Rigidbody2D rb;
//    public float dashSpeed;
//    private float dashTime;
//    public float startDashTime;
//    private int direction;

//    // Start is called before the first frame update
//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        dashTime = startDashTime;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(direction == 0)
//        {
//            if(Input.GetKeyDown(KeyCode.LeftArrow))
//            {
//                direction = 1;
//            }
//            else if(Input.GetKeyDown(KeyCode.RightArrow))
//            {
//                direction = 2;
//            }
//            else if (Input.GetKeyDown(KeyCode.UpArrow))
//            {
//                direction = 3;
//            }
//            else if (Input.GetKeyDown(KeyCode.DownArrow))
//            {
//                direction = 4;
//            }
//        }
//        else
//        {
//            if(dashTime <= 0)
//            {
//                direction = 0;
//                dashTime = startDashTime;
//                rb.velocity = Vector2.zero;
//            }
//            else
//            {
//                dashTime -= Time.deltaTime;

//                if(direction == 1)
//                {
//                    rb.velocity = Vector2.left * dashSpeed;

//                }
//                if (direction == 2)
//                {
//                    rb.velocity = Vector2.right * dashSpeed;

//                }
//                if (direction == 3)
//                {
//                    rb.velocity = Vector2.up * dashSpeed;

//                }
//                if (direction == 4)
//                {
//                    rb.velocity = Vector2.down * dashSpeed;

//                }
//            }
//        }
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EunjinHong_DashMove : MonoBehaviour
{
    private GameObject Player;
    private PlayerMove move;
    private float timer = 0.0f;
    private bool isDashing = false;
    private const float dashTime = 0.25f;

    public float dashSpeed = 3.0f;
    private float baseSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        move = Player.GetComponent<PlayerMove>();
        baseSpeed = 3.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDashing == false)
        {
            isDashing = Input.GetKey(KeyCode.F);
            timer = 0.0f;

        }
        if (isDashing)
        {
            timer += Time.fixedDeltaTime;
            move.speed = dashSpeed;
            if (timer >= dashTime)
            {
                isDashing = false;
                move.speed = baseSpeed;
            }
        }
    }
}




