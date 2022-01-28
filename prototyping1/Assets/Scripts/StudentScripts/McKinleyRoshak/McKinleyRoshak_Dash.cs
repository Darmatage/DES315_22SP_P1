using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McKinleyRoshak_Dash : MonoBehaviour
{
    private GameObject Player;
    private PlayerMove move;
    private float timer = 0.0f;
    private bool isDashing = false;
    public float dashTime = 0.25f;
    public float cooldownTime = 0.33f;

    public float dashSpeed = 3.0f;
    private float baseSpeed;

    public GameObject LavaObject;
    private Lava lavaScript;

    public KeyCode DashKey = KeyCode.Q;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        move = Player.GetComponent<PlayerMove>();
        baseSpeed = 5.0f;
        lavaScript = LavaObject.GetComponent<Lava>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (isDashing == false)
        {
            if(timer >= cooldownTime && Input.GetKey(DashKey))
            {
                isDashing = true;
                timer = 0.0f;
                lavaScript.damage = 0;
            }
        }
        if(isDashing)
        {
            
            move.speed = dashSpeed;
            if (timer >= dashTime)
            {
                isDashing = false;
                move.speed = baseSpeed;
                timer = 0.0f;
                lavaScript.damage = 1;
            }
        }
    }
}




