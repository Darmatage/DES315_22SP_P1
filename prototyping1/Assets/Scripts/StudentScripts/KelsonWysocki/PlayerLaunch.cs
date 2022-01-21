using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaunch : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private PlayerMove pMove;

    private Vector2 launchPos;
    private Vector2 launchStartPos;

    private float originalPlayerMoveSpeed;

    public float launchSpeed;
    public float launchDistance;
    public float launchTime;

    private float launchStartTime;
    private bool isLaunching = false;

    private void Start()
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody2D>();
        pMove = player.GetComponent<PlayerMove>();

        originalPlayerMoveSpeed = pMove.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            LaunchPlayer();

        Vector3 cursorPos = new Vector2(player.transform.position.x, player.transform.position.y) + (FindCursorDirection() * Mathf.Clamp(GetPlayerToCursor().magnitude, 0f, launchDistance));
        transform.position = cursorPos;
    }

    void FixedUpdate()
    {
        if (isLaunching)
            Launching();
    }

    private Vector2 GetPlayerToCursor()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = player.transform.position;

        Vector2 launchDir = mousePos - playerPos;

        return launchDir;
    }

    private Vector2 FindCursorDirection()
    {
        Vector2 launchDir = GetPlayerToCursor();
        launchDir.Normalize();

        return launchDir;
    }

    private void LaunchPlayer()
    {
        if (isLaunching) return;

        launchPos = (transform.position - player.transform.position);
        launchPos.Normalize();

        isLaunching = true;
        launchStartTime = Time.deltaTime;
        launchStartPos = player.transform.position;

        player.tag = "bullet";

        pMove.speed = 0f;

        Invoke(nameof(EndLaunch), launchTime);

        Debug.DrawRay(launchStartPos, launchPos * launchDistance, Color.red, launchTime, false);
    }

    private void Launching()
    {
        rb.velocity = launchPos * Mathf.Lerp(0f, launchSpeed, launchSpeed * Time.deltaTime);
    }

    private void EndLaunch()
    {
        isLaunching = false;
        pMove.speed = originalPlayerMoveSpeed;
        rb.velocity = Vector2.zero;
        player.tag = "Player";
    }
}
