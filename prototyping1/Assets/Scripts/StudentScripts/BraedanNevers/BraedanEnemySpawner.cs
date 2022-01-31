using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraedanEnemySpawner : MonoBehaviour
{

    public GameObject EnemyToSpawn;
    public int spawnNum;
    
    public bool spawnOnTimer = false;   // Will spawn an enemy when SpawnTime reaches 0, turns off spawnOnDestroy
    public float SpawnTime;
    
    public bool spawnOnDestroy = false; // Will spawn an enemy when the previous spawned enemy is killed, turns off spawnOnTimer
    
    public bool spawnInRange = false;   // Will spawn enemy when the player's distance from spawner is less than spawnRange
    public float spawnRange;

    private GameObject respawnEnemy;
    private float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (spawnOnTimer)
        {
            if(timer < 0.0f && spawnNum > 0)
            {
                Instantiate(EnemyToSpawn, transform.position, Quaternion.identity);
                timer = SpawnTime;
                --spawnNum;
            }
            timer -= Time.deltaTime;
            spawnOnDestroy = false;
            spawnInRange = false;
        }
        else if(spawnOnDestroy)
        {
            if(!respawnEnemy && spawnNum > 0)
            {
                respawnEnemy = Instantiate(EnemyToSpawn, transform.position, Quaternion.identity);
                --spawnNum;
            }
            spawnOnTimer = false;
            spawnInRange = false;
        }
        else if(spawnInRange)
        {
            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            if(Vector2.Distance(playerPos, transform.position) <= spawnRange)
            {
                while(spawnNum > 0)
                {
                    // It's just called multi slime cause that's what I used for one spawnner, and i need to turn it on
                    // since I cant edit the prefab and need one with changed values for my scene
                    GameObject multiSlime = Instantiate(EnemyToSpawn, transform.position, Quaternion.identity);
                    multiSlime.SetActive(true);
                    --spawnNum;
                }
            }
            spawnOnDestroy = false;
            spawnOnTimer = false;
        }
    }
}
