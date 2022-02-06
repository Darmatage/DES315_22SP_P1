using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kick : MonoBehaviour
{
    public GameObject KickInstance;
    public Animator PlayerAnimator;
    public SpriteRenderer PlayerSprite;

    [Tooltip("Use the original kick behavior that doesn't stop the player or build up power by holding down LMB.")]
    public bool UseOldKick = true;

    [Tooltip("Sets the enemy stunned state when kicked.")]
    public bool StunEnemies = false;
    [Tooltip("The amount of time to stun enemies, in seconds.")]
    public float StunTime = 2f;

    [Tooltip("The speed, size, and distance of the kick whoosh. 3 is the default for the original kick behavior.")]
    public float Force = 3f;
    [Tooltip("When using the new kick behavior, how high can the force go?")]
    public float MaxForce = 8f;

    private bool kicking = false;

    private Text text;

    void Start()
    {
        text = GetComponentInChildren<Text>();
        text.text = "";
        if (!UseOldKick)
        {
            // FixedUpdate updates at 0.02 sec/frame but stun decreases by only 0.01 sec/frame
            Force = 2f;
        }
    }

    void Update()
    {
        // account for the player script flipping the player with the scale
        if (transform.parent.localScale.x < 0)
            transform.localScale = new Vector3(-1, 1);
        else
           transform.localScale = new Vector3(1, 1);

        
        if (Input.GetMouseButtonDown(0))
        {
            if (UseOldKick)
            {
                GameObject kickInst = GameObject.Instantiate(KickInstance, transform, false);

                Vector2 off = Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition;
                float z = Mathf.Atan2(off.y, off.x) * Mathf.Rad2Deg + 180f;

                kickInst.transform.eulerAngles = new Vector3(0f, 0f, z);
                kickInst.transform.localPosition = new Vector3(0f, 0f, 0f);
                kickInst.GetComponent<KickWhoosh>().SetPower(Force / 3f + 0.5f);
                EnemyPusher enemyPusher = kickInst.GetComponentInChildren<EnemyPusher>();
                enemyPusher.StunEnemies = StunEnemies;
                enemyPusher.StunTime = StunTime;
                PlayerAnimator.SetTrigger("Kick");
            }
            else
            {
                kicking = true;
                transform.parent.GetComponent<PlayerMove>().speed = 1f;
            }
        }

        if (kicking)
        {
            PlayerAnimator.SetTrigger("Kick");
            PlayerAnimator.speed = 0f;
            Force += Time.deltaTime * 4f;
            Force = Mathf.Clamp(Force, 0.8f, MaxForce);
            int pow = Mathf.RoundToInt(Force - 2f);
            text.text = pow.ToString();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if (!UseOldKick)
            {
                text.text = "";
                PlayerAnimator.speed = 1f;

                GameObject kickInst = GameObject.Instantiate(KickInstance, transform, true);

                Vector2 off = Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition;
                float z = Mathf.Atan2(off.y, off.x) * Mathf.Rad2Deg + 180f;

                kickInst.transform.eulerAngles = new Vector3(0f, 0f, z);
                kickInst.transform.position = transform.position;
                kickInst.GetComponent<KickWhoosh>().SetPower(Force / 3f);
                EnemyPusher enemyPusher = kickInst.GetComponentInChildren<EnemyPusher>();
                enemyPusher.StunEnemies = StunEnemies;
                enemyPusher.StunTime = StunTime;
                enemyPusher.Force = 2f * Force;
                enemyPusher.LinearPush = true;
                enemyPusher.LinearPushDir = -off.normalized;
                
                Force = 2f;
                transform.parent.GetComponent<PlayerMove>().speed = 3f;
                kicking = false;
            }
        }

    }

}
