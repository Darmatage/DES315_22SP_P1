using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McKinleyRoshak_Dash : MonoBehaviour
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
        if(isDashing == false)
        {
            isDashing = Input.GetKey(KeyCode.F);
            timer = 0.0f;
            
        }
        if(isDashing)
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




