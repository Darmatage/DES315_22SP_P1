using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreddyMartinMirrorEnemy : MonoBehaviour
{
    public GameObject MirrorShardsPrefab;
    public GameObject SwapBeamPrefab;

    [HideInInspector]
    public Vector3 mirrorPoint;
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public float timeBetweenSwaps;
    [HideInInspector]
    public int shardsBetweenSwaps;
    [HideInInspector]
    public float shardFadeTime;
    [HideInInspector]
    public GameObject swapBeam;

    GameObject player;
    GameObject swapTimerText;
    SpriteRenderer playerRenderer;
    SpriteRenderer sR;

    float dropTime;
    float dropTimer = 0;
    float swapTimer = 0;
    int droppedShards = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Set swap timer text
        swapTimerText = GameObject.Find("SwapTimer");

        // Set player vars
        player = GameObject.FindGameObjectWithTag("Player");
        playerRenderer = player.GetComponentInChildren<SpriteRenderer>();

        // Get self vars
        sR = GetComponent<SpriteRenderer>();

        // Set drop time
        dropTime = timeBetweenSwaps / (shardsBetweenSwaps + 1);

        // Make swap beam
        swapBeam = Instantiate(SwapBeamPrefab, mirrorPoint, Quaternion.identity);
        swapBeam.transform.LookAt(transform, Vector3.back);
        swapBeam.transform.eulerAngles += new Vector3(90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Find mirrored position
        Vector2 enemyOffset = mirrorPoint - player.transform.position;
        Vector3 V3Offset = new Vector3(enemyOffset.x, enemyOffset.y, 0);
        transform.position = mirrorPoint + V3Offset;

        // Set mirrored facing
        Vector3 enemyScale = transform.localScale;
        enemyScale.x = -player.transform.localScale.x;
        transform.localScale = enemyScale;

        // Mirror animations
        sR.sprite = playerRenderer.sprite;

        // Update timers
        dropTimer += Time.deltaTime;
        swapTimer += Time.deltaTime;

        // Spawn shard if enough time has passed
        if (droppedShards < shardsBetweenSwaps && dropTimer >= dropTime)
        {
            ++droppedShards;
            dropTimer -= dropTime;
            GameObject shard = Instantiate(MirrorShardsPrefab, transform.position, Quaternion.identity);
            shard.GetComponent<FreddyMartinMirrorShards>().fadeTime = shardFadeTime;
            shard.GetComponent<Lava>().damage = damage;
        }

        // Swap player and mirror if enough time has passed or if the player forces a swap
        if (FreddyMartinPlayerScript.Instance.forceSwap || swapTimer >= timeBetweenSwaps)
        {
            swapTimer = 0;
            droppedShards = 0;
            dropTimer = 0;

            Vector3 newPlayerPos = transform.position;
            Vector3 newPlayerScale = transform.localScale;

            transform.position = player.transform.position;
            transform.localScale = player.transform.localScale;

            Vector3 CameraOffset = Camera.main.transform.position - player.transform.position;
            player.transform.position = newPlayerPos;
            Camera.main.transform.position = player.transform.position + CameraOffset;
            player.transform.localScale = newPlayerScale;
        }

        // Move swap beam
        swapBeam.transform.position = (transform.position + player.transform.position) / 2.0f;
        swapBeam.transform.LookAt(transform, Vector3.back);
        swapBeam.transform.eulerAngles += new Vector3(90, 0, 0);

        ParticleSystem beamSystem = swapBeam.GetComponent<ParticleSystem>();

        // Resize swap beam
        var shape = beamSystem.shape;
        shape.radius = Vector3.Distance(player.transform.position, transform.position) / 2.0f;

        // Set emission rate and color
        var emission = beamSystem.emission;
        emission.rateOverTime = 180.0f * (swapTimer / timeBetweenSwaps) *
            Vector3.Distance(player.transform.position, transform.position);

        var main = beamSystem.main;

        if (timeBetweenSwaps - swapTimer <= 0.75f)
        {
            main.startColor = Color.red;
        }
        else
        {
            main.startColor = (swapTimer / (timeBetweenSwaps - 0.75f)) * Color.yellow +
                (1 - swapTimer / (timeBetweenSwaps - 0.75f)) * Color.cyan;
        }

        if (swapTimerText)
        {
            swapTimerText.GetComponent<Text>().text = ((timeBetweenSwaps - swapTimer) - ((timeBetweenSwaps - swapTimer) % 0.1f)).ToString();
        }
    }

    private void OnDestroy()
    {
        if (swapTimerText)
        {
            swapTimerText.GetComponent<Text>().text = "";
        }
    }
}
