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
    }

    protected override void FixedUpdate()
    {
        // Do nothing
    }

	protected override void UpdateAnimationAndMove() {
		// Do nothing
	}

    public void MovePlayer(Vector3 move)
    {
        rb.MovePosition(transform.position + move);
    }
}
