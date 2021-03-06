using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomePlayerControllerScript : PlayerMove
{
    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    protected override void FixedUpdate()
    {
        
    }

	protected override void UpdateAnimationAndMove() {
		// Do nothing
	}

    public void MovePlayer(Vector3 move)
    {
        rb.MovePosition(transform.position + move);
    }
}
