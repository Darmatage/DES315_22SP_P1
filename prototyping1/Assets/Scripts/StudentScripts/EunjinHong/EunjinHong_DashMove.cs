using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EunjinHong_DashMove : MonoBehaviour
{
    private GameObject Player;
    private PlayerMove move;
    public float timer = 0.0f;  //timer for dashing
    public bool isDashing = false;

    public float dashGauge = 50;
    public float dashTime = 0.05f;

    public float dashSpeed = 10.0f;
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
            isDashing = Input.GetKey(KeyCode.LeftShift);
            timer = 0.0f;   //set timer to 0 therefore it can start dashing
        }
        if (isDashing == true)
        {


            timer += Time.fixedDeltaTime;
            if(dashGauge > 0)
            {
                dashGauge--;
                timer += Time.fixedDeltaTime;
                move.speed = dashSpeed;
            }

            if (timer >= dashTime)
            {
                isDashing = false;
                move.speed = baseSpeed;
            }
            //dash for 0.05 delta seconds
        }
    }

}




