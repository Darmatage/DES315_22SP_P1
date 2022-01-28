using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeanteJames_TimerLogic : MonoBehaviour
{
    private Text timerText;
    private List<GameObject> allEnemiesInScene = new List<GameObject>();

    // Set in inspector
    public Color startColor = Color.green;
    public Color endColor = Color.red;

    // format MM::SS
    public string timeLevelStopsGettingHard;

    //public float mediumPoint = 1.75f;
    //public float hardPoint = 2.50f;

    //private float pointToLerpTo = 0.0f;
    private float speedToLerp = 0.05f;

    // skull characteristics to change
    public float skull_hardestSpeed = 5.0f;
    public float skull_hardestStoppingDistance = 5.0f;
    public float skull_hardestStartTimeBtwShots = 0.55f;
    public int skull_hardestDmg = 6;
    public GameObject skull_projectile;

    // slime characteristics to change
    public float slime_hardestSpeed = 6.5f;
    public int slime_hardestDmg = 6;
    public float slime_retreatDist = 1.0f;

    private bool midPointReached = false;
    private bool hardestPointReached = false;

    // Lava tile spawning
    public Tilemap lavaMap;
    public Tile lavaTile;
    private int startNumOfTilesToSpawn = 1;
    public float spawnTilesIntervalPerSecond = 0;
    public int howManyTilesPerInterval = 1;
    public bool increment_HowManyTiles_WhenSpawned = false;
    private int prev = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Text Box to Display the time
        timerText = gameObject.GetComponent<Text>();
        timerText.color = startColor;

        // All the enemies in the scene
        allEnemiesInScene.Add(GameObject.Find("MonsterSkull"));
        allEnemiesInScene.Add(GameObject.Find("MonsterSlime"));
        //Time.timeScale = startingScale;
        //pointToLerpTo = mediumPoint;
    }

    // Update is called once per frame
    void Update()
    {
        float timeElasped = (Time.timeSinceLevelLoad);
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

        float totalTime = (getMinutes() * 60) + getSeconds();
        speedToLerp = Mathf.Abs(totalTime - timeElasped) * (1.0f/4000.0f);
        minutes += ((int)mins).ToString();
        seconds += ((int)secs).ToString();
        milliSeconds = Mathf.RoundToInt((timeElasped * 100) % 100).ToString();
        
        timerText.text = minutes + ":" + seconds + ":" + milliSeconds;
        timerText.color = Color.Lerp(timerText.color, endColor, speedToLerp * Time.deltaTime);

        // if the mins and secs match up the 
        MakeAllEnemiesStronger(mins, secs);
        lerpCharacteristics(speedToLerp);

        int seconds_int = (int)secs;
        if (seconds_int % spawnTilesIntervalPerSecond == 0 && seconds_int != prev)
        {
            spawnLavaTile(startNumOfTilesToSpawn);
            prev = seconds_int;
        }
    }

    void MakeAllEnemiesStronger(float min, float sec)
    {
        if ((Mathf.Approximately(min, getMinutes()/ 2.0f)) && (Mathf.Approximately(sec, getSeconds()/ 2.0f)))
        {
            midPointReached = true;
        }

        if ((Mathf.Approximately(min, getMinutes())) && (Mathf.Approximately(sec, getSeconds())))
        {
            hardestPointReached = true;
        }


        //if (!(Mathf.Approximately(min, getMinutes())) && !(Mathf.Approximately(sec, getSeconds())))
        //{ return; }

        // Base enemies go faster
        // 1. Increase the speed of the
        // 2. Decrease time inbetween shots
        foreach (GameObject item in allEnemiesInScene)
        {
            if (item.name.Contains("Slime"))
            {
                MonsterMoveHit slimeDMG = item.GetComponent<MonsterMoveHit>();
                if (midPointReached)
                {
                    slimeDMG.damage = slime_hardestDmg / 2;
                }
                else if (hardestPointReached)
                {
                    slimeDMG.damage = slime_hardestDmg;
                }
            }

            if (item.name.Contains("Skull"))
            {
                Projectile proj = item.gameObject.GetComponent<Projectile>();
                if (midPointReached)
                {
                    proj.damage = skull_hardestDmg / 2;
                }
                else if (hardestPointReached)
                {
                    proj.damage = skull_hardestDmg;    
                }
            }
        }

        if (midPointReached)
        {
            midPointReached = false;
        }

        if (hardestPointReached)
        {
            hardestPointReached = false;
        }
    }

    private void lerpCharacteristics(float t)
    {
        foreach (GameObject item in allEnemiesInScene)
        {
            if (item.name.Contains("Slime"))
            {
                MonsterMoveHit slime = item.GetComponent<MonsterMoveHit>();
                slime.speed = Mathf.Lerp(slime.speed, slime_hardestDmg, speedToLerp * Time.deltaTime);
                slime.retreatTime = Mathf.Lerp(slime.retreatTime, slime_retreatDist, speedToLerp * Time.deltaTime);
            }

            if (item.name.Contains("Skull"))
            {
                MonsterShootMove skull = item.gameObject.GetComponent<MonsterShootMove>();
                skull.speed = Mathf.Lerp(skull.speed, skull_hardestDmg, speedToLerp * Time.deltaTime);
                skull.startTimeBtwShots = Mathf.Lerp(skull.startTimeBtwShots, skull_hardestStartTimeBtwShots, speedToLerp * Time.deltaTime);
                skull.stoppingDistance = Mathf.Lerp(skull.stoppingDistance, skull_hardestStoppingDistance, speedToLerp * Time.deltaTime);
            }
        }
    }

    private float getMinutes()
    {
        float min = float.Parse(timeLevelStopsGettingHard.Substring(0, 2));
        return min;
    }

    private float getSeconds()
    {
        float sec = float.Parse(timeLevelStopsGettingHard.Substring(3, 2));
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

    public Vector3Int Random(Vector3Int min, Vector3Int max)
    {
        return new Vector3Int(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
    }
}
