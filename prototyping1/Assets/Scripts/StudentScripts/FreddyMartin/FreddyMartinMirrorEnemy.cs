using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    SpriteRenderer playerRenderer;
    SpriteRenderer sR;

    float dropTime;
    float dropTimer = 0;
    float swapTimer = 0;
    int droppedShards = 0;

    // Start is called before the first frame update
    void Start()
    {
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

        // Swap player and mirror if enough time has passed
        if (swapTimer >= timeBetweenSwaps)
        {
            swapTimer -= timeBetweenSwaps;
            droppedShards = 0;
            dropTimer = 0;

            Vector3 newPlayerPos = transform.position;
            Vector3 newPlayerScale = transform.localScale;

            transform.position = player.transform.position;
            transform.localScale = player.transform.localScale;

            player.transform.position = newPlayerPos;
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

        // Set emission rate
        var emission = beamSystem.emission;
        emission.rateOverTime = 180.0f * (swapTimer / timeBetweenSwaps) *
            Vector3.Distance(player.transform.position, transform.position);
    }
}