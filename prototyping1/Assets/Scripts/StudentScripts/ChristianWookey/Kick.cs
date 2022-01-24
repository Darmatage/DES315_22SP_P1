using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour
{
    public GameObject ArtPivot;
    public SpriteRenderer Art;
    public Animator PlayerAnimator;
    public float KickTime = 0.35f;
    public float KickDelay = 0.15f;


    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
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
            Vector2 off = Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition;
            float z = Mathf.Atan2(off.y, off.x) * Mathf.Rad2Deg + 180f;
            ArtPivot.transform.eulerAngles = new Vector3(0f, 0f, z );
            animator.SetTrigger("Kick");
            PlayerAnimator.SetTrigger("Kick");
        }
    }

}
