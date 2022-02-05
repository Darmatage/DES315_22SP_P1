using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class DeanteJames_TimerLogic : MonoBehaviour
{
    private Text timerText;

    [SerializeField]
    public GameObject flyingText;

    // Children
    public GameObject indicatorArrow;
    public GameObject indicatorBar;
    public float unitsToTheRight = 0.0f;



    //private List<GameObject> allEnemiesInScene = new List<GameObject>();

    private bool GameHasStarted = false;
    private bool GameIsPaused = false;
    private GameObject toDelete = null;

    public float coinDeductionLength = 10.0f;
    public float coinDeductionTimer = 0.0f;
    // Set in inspector
    public Color startColor = Color.green;
    public Color endColor = Color.red;
    private Color colorLastLerped;
    private float origTimeScale = 0.0f;

    [SerializeField]
    public GameObject wave_manager;

    // format MM::SS
    public string timeForHardDifficulty;
    public string timeForMedDifficulty;
    private float timeDeductions = 0.0f;
    private float timeElasped = 0.0f;


    private float speedToLerp = 0.05f;

    // skull characteristics to change
    public float skull_hardSpeed = 5.0f;
    public float skull_hardStoppingDistance = 5.0f;
    public float skull_hardStartTimeBtwShots = 0.55f;
    public int skull_hardDmg = 6;

    public float skull_medSpeed = 5.0f;
    public float skull_medStoppingDistance = 5.0f;
    public float skull_medStartTimeBtwShots = 0.55f;
    public int skull_medDmg = 6;

    public GameObject skull_projectile;



    // slime characteristics to change
    // The default values is easy mode
    public float slime_hardSpeed = 6.5f;
    public int slime_hardDmg = 6;
    public float slime_hardRetreatTime = 1.0f;

    public float slime_medSpeed = 6.5f;
    public int slime_medDmg = 6;
    public float slime_medRetreatTime = 1.0f;


    private bool midPointReached = false;
    private bool hardestPointReached = false;

    // Boss slime characteristics
    public float bossSlime_hardSpeed = 6.5f;
    public int bossSlime_hardDmg = 6;
    public float bossSlime_hardRetreatTime = 1.0f;

    public float bossSlime_medSpeed = 6.5f;
    public int bossSlime_medDmg = 6;
    public float bossSlime_medRetreatTime = 1.0f;

    // Lava tile spawning
    public Tilemap lavaMap;
    public Tile lavaTile;
    private int startNumOfTilesToSpawn = 1;
    public float spawnTilesIntervalPerSecond = 0;
    public int howManyTilesPerInterval = 1;
    public bool increment_HowManyTiles_WhenSpawned = false;
    private int prev = 0;


    // abilites
    private bool timeFreeze = false;
    public float timeFreezeDuration = 0.0f;
    private float timer = 0.0f;
    public Color freezeColor = Color.cyan;

    public float animationLength = 1.5f;
    private float animationTimer = 1.5f;
    private int timesAnimationHasBeenPlayed = 0;
    private bool play = false;

    private Coroutine coroutineStarted;
    // Start is called before the first frame update
    void Start()
    {
        // Text Box to Display the time
        timerText = gameObject.GetComponent<Text>();
        timerText.color = startColor;
        animationTimer = animationLength;
        colorLastLerped = timerText.color;
        origTimeScale = Time.timeScale;

        HideDifficultyBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !GameHasStarted)
        { 
            GameHasStarted = true;

            ShowDifficultyBar();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (GameIsPaused)
            {
                GameIsPaused = false;
                unPauseGame();
            }
        }

        if (!GameHasStarted || GameIsPaused)
        { return; }

        timeElasped += Time.deltaTime - timeDeductions;

        UpdateFreezeData();

        if (timeElasped < 0.0f)
        {
            timeElasped = 0.0f;
        }

        string minutes = "";
        string seconds = "";
        string milliSeconds = "";
        float secs = timeElasped % 60;
        float mins = timeElasped / 60;
        if (!((int)timeElasped % 60 > 9))
        {
            seconds = "0";
        }

        if (!((int)timeElasped / 60 > 9))
        {
            minutes = "0";
        }

        if (!timeFreeze)
        {
            float totalTime = (getMinutes(timeForHardDifficulty) * 60) + getSeconds(timeForHardDifficulty);
            speedToLerp = Mathf.Abs(totalTime - timeElasped) * (1.0f / 11000.0f);
            minutes += ((int)mins).ToString();
            seconds += ((int)secs).ToString();
            milliSeconds = Mathf.RoundToInt((timeElasped * 100) % 100).ToString();

            timerText.text = minutes + ":" + seconds + ":" + milliSeconds;
            timerText.color = Color.Lerp(colorLastLerped, endColor, speedToLerp * Time.deltaTime);

            if (timeDeductions <= 0.0f)
            {
                updateIndicatorArrow(speedToLerp, unitsToTheRight);
            }
            else
            {
                // lerp the arrow indicator back for x amount of seconds
                updateIndicatorArrow(speedToLerp, -(timeDeductions + unitsToTheRight));
                coinDeductionTimer -= Time.deltaTime;
                if (coinDeductionTimer <= 0.0f)
                {
                    timeDeductions = 0.0f;
                }
            }

            colorLastLerped = timerText.color;
        }

        // are there any enemies left in the wave?
        if (Enumerable.Any(wave_manager.GetComponent<JirakitJarusiripipat_GameManager>().allEnemy))
        {
            // if the mins and secs match up the 
            MakeAllEnemiesStronger(mins, secs);
        }

        int seconds_int = (int)secs;
        if (seconds_int % spawnTilesIntervalPerSecond == 0 && seconds_int != prev)
        {
            spawnLavaTile(startNumOfTilesToSpawn);
            prev = seconds_int;
        }

        CheckAnimation(secs, mins);
    }

    private void UpdateFreezeData()
    {
        if (timeFreeze)
        {
            timer -= Time.deltaTime;
            timerText.color = Color.Lerp(timerText.color, freezeColor, 0.5f * Time.deltaTime);
        }

        if (timer <= 0.0f)
        {
            timeFreeze = false;
        }
    }

    private void CheckAnimation(float secs, float mins)
    {
        if ( (( (int)mins == (int)getMinutes(timeForMedDifficulty)) && (Mathf.Approximately(Mathf.Round(secs), getSeconds(timeForMedDifficulty))))
            && timesAnimationHasBeenPlayed == 0)
        {
            ++timesAnimationHasBeenPlayed;
            animationTimer = animationLength;
            PlayBlinkAnimation();
            play = true;
        }

        if ( (Mathf.Approximately(Mathf.Round(mins), getMinutes(timeForHardDifficulty)) && Mathf.Approximately(Mathf.Round(secs), getSeconds(timeForHardDifficulty)))
            && timesAnimationHasBeenPlayed == 1)
        {
            ++timesAnimationHasBeenPlayed;
            animationTimer = animationLength;
            PlayBlinkAnimation();
            play = true;
        }

        if (play)
        {
            animationTimer -= Time.deltaTime;
        }

        if (animationTimer <= 0.0f)
        {
            StopBlinkAnimation();
            play = false;
        }
    }

    void MakeAllEnemiesStronger(float min, float sec)
    {
        if ((int)min == (int)getMinutes(timeForMedDifficulty) && (int)sec == (int)getSeconds(timeForMedDifficulty))
        {
            midPointReached = true;
        }

        if ((int)min == (int)getMinutes(timeForHardDifficulty) && (Mathf.Approximately(Mathf.Round(sec), getSeconds(timeForHardDifficulty))))
        {
            hardestPointReached = true;
        }

        // Base enemies go faster
        // 1. Increase the speed of the
        // 2. Decrease time inbetween shots
        foreach (GameObject item in wave_manager.GetComponent<JirakitJarusiripipat_GameManager>().allEnemy)
        {
            if (item == null)
            {
                continue;
            }

            // boss slime
            if (item.name.Contains("Slime2"))
            {
                JirakitJarusiripipat_MonsterMoveHit slimeDMG = item.GetComponent<JirakitJarusiripipat_MonsterMoveHit>();
                if (midPointReached)
                {
                    slimeDMG.damage = slime_medDmg;
                    slimeDMG.speed = slime_medSpeed;
                    slimeDMG.retreatTime = slime_medRetreatTime;
                }
                else if (hardestPointReached)
                {
                    slimeDMG.damage = slime_hardDmg;
                    slimeDMG.speed = slime_hardSpeed;
                    slimeDMG.retreatTime = slime_hardRetreatTime;
                }
            }
            else if (item.name.Contains("Slime"))
            {
                JirakitJarusiripipat_MonsterMoveHit slimeDMG = item.GetComponent<JirakitJarusiripipat_MonsterMoveHit>();
                if (midPointReached)
                {
                    slimeDMG.damage = bossSlime_medDmg;
                    slimeDMG.speed = bossSlime_medSpeed;
                    slimeDMG.retreatTime = bossSlime_medRetreatTime;
                }
                else if (hardestPointReached)
                {
                    slimeDMG.damage = bossSlime_hardDmg;
                    slimeDMG.speed = bossSlime_hardSpeed;
                    slimeDMG.retreatTime = bossSlime_hardRetreatTime;
                }
            }
            else if (item.name.Contains("Skull"))
            {
                JirakitJarusiripipat_Projectile proj = item.gameObject.GetComponent<JirakitJarusiripipat_Projectile>();
                JirakitJarusiripipat_ShootMove skull = item.GetComponent<JirakitJarusiripipat_ShootMove>();

                if (proj == null)
                {
                    continue;
                }

                if (midPointReached)
                {
                    proj.damage = skull_medDmg;
                    skull.stoppingDistance = skull_medStoppingDistance;
                    skull.startTimeBtwShots = skull_medStartTimeBtwShots;
                    skull.speed = skull_medSpeed;
                }
                else if (hardestPointReached)
                {
                    proj.damage = skull_hardDmg;
                    skull.stoppingDistance = skull_hardStoppingDistance;
                    skull.startTimeBtwShots = skull_hardStartTimeBtwShots;
                    skull.speed = skull_hardSpeed;
                }
            }
        }
    }

    //private void lerpCharacteristics(float t)
    //{
    //    foreach (GameObject item in wave_manager.GetComponent<JirakitJarusiripipat_GameManager>().allEnemy)
    //    {
    //        if (item == null)
    //        {
    //            continue;
    //        }

    //        if (item.name.Contains("Slime"))
    //        {
    //            JirakitJarusiripipat_MonsterMoveHit slime = item.GetComponent<JirakitJarusiripipat_MonsterMoveHit>();
    //            slime.speed = Mathf.Lerp(slime.speed, slime_hardestSpeed, speedToLerp * Time.deltaTime);
    //            slime.retreatTime = Mathf.Lerp(slime.retreatTime, slime_hardRetreatDist, speedToLerp * Time.deltaTime);
    //        }

    //        if (item.name.Contains("Skull"))
    //        {
    //            JirakitJarusiripipat_ShootMove skull = item.gameObject.GetComponent<JirakitJarusiripipat_ShootMove>();
    //            skull.speed = Mathf.Lerp(skull.speed, skull_hardestDmg, speedToLerp * Time.deltaTime);
    //            skull.startTimeBtwShots = Mathf.Lerp(skull.startTimeBtwShots, skull_hardestStartTimeBtwShots, speedToLerp * Time.deltaTime);
    //            skull.stoppingDistance = Mathf.Lerp(skull.stoppingDistance, skull_hardestStoppingDistance, speedToLerp * Time.deltaTime);
    //        }
    //    }
    //}

    private float getMinutes(string time)
    {
        float min = float.Parse(time.Substring(0, 2));
        return min;
    }

    private float getSeconds(string time)
    {
        float sec = float.Parse(time.Substring(3, 2));
        return sec;
    }

    private void spawnLavaTile(int numOfTilesToSpawn)
    {
        Grid tiles = lavaMap.GetComponent<Grid>();
        BoundsInt bound = lavaMap.cellBounds;

        for (int i = 0; i < numOfTilesToSpawn; ++i)
        {
            Vector3Int vec = Random(new Vector3Int(bound.xMin, bound.yMin, bound.zMin), new Vector3Int(bound.xMax, bound.yMax, bound.zMax));
            lavaMap.SetTile(vec, lavaTile);
        }

        if (increment_HowManyTiles_WhenSpawned)
        {
            howManyTilesPerInterval += howManyTilesPerInterval;
        }
    }

    public void AddDeduction(float toAdd)
    {
        timeDeductions += toAdd;
        GameObject obj = GameObject.Instantiate(flyingText, timerText.transform);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        Text numbers = obj.GetComponent<Text>();
        numbers.text = "-"+ toAdd.ToString();
        numbers.fontSize = 50;
        numbers.color = Color.yellow;

        Vector3 randVel = new Vector3(20.0f, 45.0f);
        rb.velocity = randVel;
        rb.gravityScale = 5.5f;
        rb.AddForce(new Vector3(2.0f, 2.0f));

        coinDeductionTimer = coinDeductionLength;
    }

    public void FreezeTime(bool freeze)
    {
        timeFreeze = freeze;
        timer = timeFreezeDuration;
    }

    public Vector3Int Random(Vector3Int min, Vector3Int max)
    {
        return new Vector3Int(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
    }

    private void PlayBlinkAnimation()
    {
        coroutineStarted = StartCoroutine(Blink());
    }

    private void StopBlinkAnimation()
    {
        StopCoroutine(coroutineStarted);
    }

    //private bool isItTimeYet(f
    IEnumerator Blink()
    {
        while (true)
        {
            if (Mathf.Round(timerText.color.a) == 0.0f)
            {
                timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b, 1.0f);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b, 0.0f);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void HideDifficultyBar()
    {
        Image arrow = indicatorArrow.GetComponent<Image>();
        Image bar = indicatorBar.GetComponent<Image>();

        // Hide the difficulty indicator
        Color col = arrow.color;
        col.a = 0;
        arrow.color = col;

        col = bar.color;
        col.a = 0;
        bar.color = col;
    }

    private void ShowDifficultyBar()
    {
        Image arrow = indicatorArrow.GetComponent<Image>();
        Image bar = indicatorBar.GetComponent<Image>();

        // Hide the difficulty indicator
        Color col = arrow.color;
        col.a = 1;
        arrow.color = col;

        col = bar.color;
        col.a = 1;
        bar.color = col;
    }

    // Other game objects are able to pause the game
    public void pauseGame(GameObject obj)
    {
        GameIsPaused = true;
        Time.timeScale = 0.0f;
        
        // Save the tool tip object to delete later
        toDelete = obj;
    }

    private void unPauseGame()
    {
        GameIsPaused = false;
        Time.timeScale = origTimeScale;
        GameObject.Destroy(toDelete);
    }

    private void updateIndicatorArrow(float speed, float unitsToTheRight)
    {
        Vector3 right = indicatorArrow.transform.position;
        right.x += unitsToTheRight;

        indicatorArrow.transform.position = Vector3.Lerp(indicatorArrow.transform.position, right, speed * Time.deltaTime);
    }
}
