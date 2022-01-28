using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraedanEnemySpawner : MonoBehaviour
{

    public GameObject EnemyToSpawn;
    public float SpawnTime;
    
    private float timer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < 0.0f)
        {
            Instantiate(EnemyToSpawn, transform.position, Quaternion.identity);
            timer = SpawnTime;
        }
        timer -= Time.deltaTime;
    }
}
