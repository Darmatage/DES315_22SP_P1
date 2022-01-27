using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaunch : MonoBehaviour
{
    private GameObject launchEffect;
    private GameObject chargeEffect;

    private GameObject cooldownVisual;

    private Camera playerCamera;

    private GameObject damageArea;
    public GameObject damageAreaPrefab;

    private GameObject player;
    private Rigidbody2D rb;
    private PlayerMove pMove;

    private ParticleSystem launchEffectPSystem;
    private ParticleSystem chargeEffectPSystem;

    private Vector2 launchPos;
    private Vector2 launchStartPos;

    private float originalPlayerMoveSpeed;

    private float launchSpeed;
    public float launchDistance;
    private float launchTime = 0.2f;

    private float launchSpeedToUse;
    private float launchStartTime;
    private bool isLaunching = false;
    private float holdTime;
    private float launchDistanceToUse;

    private float damageAreaSize = 2f;
    private float damageAreaSizeToUse;

    private float originalCameraSize;
    private float cameraSizeToUse;

    public float cooldown;
    private bool canDash = true;
    private float trackCooldown = 0f;
    private bool cooldownGoing = false;
    private bool didCharge = false;

    private bool mouseDown = false;

    private void Start()
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody2D>();
        pMove = player.GetComponent<PlayerMove>();

        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        originalCameraSize = playerCamera.orthographicSize;
        cameraSizeToUse = playerCamera.orthographicSize;

        pMove.speed = 5f;
        originalPlayerMoveSpeed = pMove.speed;

        launchSpeed = launchDistance / launchTime;

        launchEffect = transform.GetChild(0).gameObject;
        chargeEffect = transform.GetChild(1).gameObject;
        launchEffectPSystem = launchEffect.GetComponent<ParticleSystem>();
        chargeEffectPSystem = chargeEffect.GetComponent<ParticleSystem>();

        cooldownVisual = transform.GetChild(2).gameObject;

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        launchDistanceToUse = launchDistance;

        trackCooldown = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canDash)
        {
            launchSpeedToUse = launchSpeed;
            launchDistanceToUse = launchDistance;
            damageAreaSizeToUse = damageAreaSize;
            holdTime = 0f;
            pMove.speed = 0f;
            mouseDown = true;
        }
        if (Input.GetMouseButtonUp(0) && canDash && mouseDown)
        {
            LaunchPlayer();
            launchDistanceToUse = launchDistance;
            trackCooldown = 0f;
            mouseDown = false;
        }
        else if (Input.GetMouseButton(0) && canDash && mouseDown)
        {
            holdTime += Time.deltaTime;
            if (holdTime > 0.5f)
                ChargeLaunch();

        }
        else if (cameraSizeToUse > originalCameraSize+0.1f)
        {
            cameraSizeToUse -= (cameraSizeToUse/2f) * Time.deltaTime;
        }

        if (cooldownGoing)
        {
            trackCooldown += Time.deltaTime;
            if (trackCooldown >= cooldown)
                cooldownGoing = false;
        }

        var cooldownVisualScale = cooldownVisual.transform.localScale;

        cooldownVisualScale.x = (trackCooldown/cooldown);
        cooldownVisualScale.y = (trackCooldown/cooldown);

        cooldownVisual.transform.localScale = cooldownVisualScale;

        Vector3 cursorPos = new Vector2(player.transform.position.x, player.transform.position.y) + (FindCursorDirection() * Mathf.Clamp(GetPlayerToCursor().magnitude, launchDistanceToUse, launchDistanceToUse));
        transform.position = cursorPos;

        Vector3 playerPos = player.transform.position;

        launchEffect.transform.position = playerPos;
        chargeEffect.transform.position = playerPos;

        playerCamera.orthographicSize = cameraSizeToUse;
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

    private void ChargeLaunch()
    {
        didCharge = true;
        var em = chargeEffectPSystem.emission;
        em.rateOverTime = 1000;
        if (launchSpeedToUse < launchSpeed * 2)
        {
            launchSpeedToUse += (launchSpeed*2f) * Time.deltaTime;
            damageAreaSizeToUse += 8f * Time.deltaTime;
            launchDistanceToUse += (launchDistance*2f) * Time.deltaTime;
            cameraSizeToUse += ((cameraSizeToUse/2.5f)*2f) * Time.deltaTime;

             var shape = chargeEffectPSystem.shape;
            shape.radius += 4f * Time.deltaTime;

            if (trackCooldown > 0f)
                trackCooldown -= (cooldown*2f) * Time.deltaTime;
        }

    }

    private void LaunchPlayer()
    {
        if (isLaunching) return;
        canDash = false;

        launchPos = (transform.position - player.transform.position);
        launchPos.Normalize();

        isLaunching = true;
        launchStartTime = Time.deltaTime;
        launchStartPos = player.transform.position;

        var em1 = chargeEffectPSystem.emission;
        em1.rateOverTime = 0;

        var shape = chargeEffectPSystem.shape;
        shape.radius = 0.5f;
        var em2 = launchEffectPSystem.emission;
        em2.rateOverTime = 1000;

        Invoke(nameof(EndLaunch), launchTime);
        damageArea = Instantiate(damageAreaPrefab);
        Vector2 newSize = new Vector2(damageAreaSizeToUse, damageAreaSizeToUse);
        damageArea.GetComponent<CapsuleCollider2D>().size = newSize;

        player.tag = "bullet";
    }

    private void Launching()
    {
        rb.velocity = launchPos * launchSpeedToUse;
        damageArea.transform.position = player.transform.position;
    }

    private void EndLaunch()
    {
        isLaunching = false;
        pMove.speed = originalPlayerMoveSpeed;
        rb.velocity = Vector2.zero;
        var em = launchEffectPSystem.emission;
        em.rateOverTime = 0;
        launchDistanceToUse = launchDistance;
        cooldownGoing = true;
        didCharge = false;

        player.tag = "Player";

        Destroy(damageArea);
        Invoke(nameof(EndCooldown), cooldown);
    }

    private void EndCooldown()
    {
        canDash = true;
    }
}
