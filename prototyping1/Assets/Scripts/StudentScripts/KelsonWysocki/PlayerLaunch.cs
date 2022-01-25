using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaunch : MonoBehaviour
{
    private GameObject launchEffect;
    private GameObject chargeEffect;

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

    public float launchSpeed;
    public float launchDistance;
    public float launchTime;

    private float launchSpeedToUse;
    private float launchStartTime;
    private bool isLaunching = false;
    private float holdTime;
    public float launchTimeToUse;
    public float launchDistanceToUse;

    private float damageAreaSize = 1f;
    private float damageAreaSizeToUse;

    private void Start()
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody2D>();
        pMove = player.GetComponent<PlayerMove>();

        pMove.speed = 5f;
        originalPlayerMoveSpeed = pMove.speed;

        launchEffect = transform.GetChild(0).gameObject;
        chargeEffect = transform.GetChild(1).gameObject;
        launchEffectPSystem = launchEffect.GetComponent<ParticleSystem>();
        chargeEffectPSystem = chargeEffect.GetComponent<ParticleSystem>();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        launchDistanceToUse = launchDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            launchSpeedToUse = launchSpeed;
            launchTimeToUse = launchTime;
            launchDistanceToUse = launchDistance;
            damageAreaSizeToUse = damageAreaSize;
            var em = chargeEffectPSystem.emission;
            em.rateOverTime = 1000;
            holdTime = 0f;
            pMove.speed = 0f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            LaunchPlayer();
            launchDistanceToUse = launchDistance;
        }
        else if (Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            if (holdTime > 0.5f)
                ChargeLaunch();
        }

        Vector3 cursorPos = new Vector2(player.transform.position.x, player.transform.position.y) + (FindCursorDirection() * Mathf.Clamp(GetPlayerToCursor().magnitude, 0f, launchDistanceToUse));
        transform.position = cursorPos;

        launchEffect.transform.position = player.transform.position;
        chargeEffect.transform.position = player.transform.position;
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
        if (launchSpeedToUse < launchSpeed * 2)
        {
            launchSpeedToUse += launchSpeed * Time.deltaTime;
            damageAreaSizeToUse += 5f * Time.deltaTime;
            damageAreaSizeToUse = 6f;
            launchDistanceToUse += launchDistance * Time.deltaTime;

             var shape = chargeEffectPSystem.shape;
            shape.radius += 2f * Time.deltaTime;
        }

    }

    private void LaunchPlayer()
    {
        if (isLaunching) return;

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

        Invoke(nameof(EndLaunch), launchTimeToUse);
        damageArea = Instantiate(damageAreaPrefab);
        Vector2 newSize = new Vector2(damageAreaSizeToUse, damageAreaSizeToUse);
        damageArea.GetComponent<CapsuleCollider2D>().size = newSize;
    }

    private void Launching()
    {
        rb.velocity = launchPos * launchSpeedToUse;// Mathf.Lerp(0f, launchSpeedToUse, launchSpeedToUse * Time.deltaTime);
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
        Destroy(damageArea);
    }
}
