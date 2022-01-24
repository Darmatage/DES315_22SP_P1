using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreddyMartinMirrorGenerator : MonoBehaviour
{
    public GameObject MirrorEnemyPrefab;
    public int MaxHealth = 5;
    public float Radius = 3.0f;
    public int ShardDamage = 10;
    public float TimeBetweenSwaps = 5.0f;
    public int ShardsDroppedBetweenSwaps = 2;
    public float ShardFadeTime = 10.0f;

    GameObject currEnemyInstance = null;
    int currHealth;

    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = Radius;

        transform.GetChild(0).transform.localScale = Vector3.one * (Radius / 6.0f);

        currHealth = MaxHealth;
    }

    private void Update()
    {
        if (currHealth <= 0)
        {
            if (currEnemyInstance != null)
            {
                Destroy(currEnemyInstance.GetComponent<FreddyMartinMirrorEnemy>().swapBeam);
                Destroy(currEnemyInstance);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            currEnemyInstance = Instantiate(MirrorEnemyPrefab);
            FreddyMartinMirrorEnemy mE = currEnemyInstance.GetComponent<FreddyMartinMirrorEnemy>();
            mE.mirrorPoint = transform.position + new Vector3(0, 0.6f, 0);
            mE.damage = ShardDamage;
            mE.timeBetweenSwaps = TimeBetweenSwaps;
            mE.shardsBetweenSwaps = ShardsDroppedBetweenSwaps;
            mE.shardFadeTime = ShardFadeTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(currEnemyInstance.GetComponent<FreddyMartinMirrorEnemy>().swapBeam);
            Destroy(currEnemyInstance);
            currEnemyInstance = null;
        }
    }

    public void Damage(int damage)
    {
        currHealth -= damage;
    }
}
