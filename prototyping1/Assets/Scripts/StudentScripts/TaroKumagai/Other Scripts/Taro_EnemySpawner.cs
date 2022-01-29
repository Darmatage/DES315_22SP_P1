using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taro_EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    public float ScanRadius = 10f;
    public float ScanInterval = 0.2f;
    public LayerMask ScanLayer;

    // Start is called before the first frame update
    void Start()
    {
        Enemy.SetActive(false);
        StartCoroutine("ScanForPlayer");
    }


    IEnumerator ScanForPlayer()
    {
        while(true)
        {
            var targets = Physics2D.OverlapCircleAll (gameObject.transform.position, ScanRadius, ScanLayer);
            
            if (targets.Length > 0)
            {
                Enemy.SetActive(true);
                break;
                // Perform some action here
            }
            yield return new WaitForSeconds(ScanInterval);
        }
    }
}
