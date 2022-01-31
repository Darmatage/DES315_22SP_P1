using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenThompson_TurnTNTSpawner : MonoBehaviour
{
    public GameObject TNT;
    private bool oneTimeCollision = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" || oneTimeCollision == false)
            return;
        else
        {
            oneTimeCollision = false;
        }

        // Get the children of the object
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child == transform)
                continue;
            else
            {
                GameObject tnt = Instantiate(TNT);
                tnt.transform.position = new Vector3(child.position.x, child.position.y, 0);

                Transform[] tntChildren = tnt.GetComponentsInChildren<Transform>();
                foreach (Transform tntChild in tntChildren)
                {
                    if (tntChild == tnt.transform)
                        continue;
                    else
                    {
                        SpriteRenderer sp = tntChild.GetComponent<SpriteRenderer>();
                        sp.sortingOrder = 110;
                    }
                }

                ExplosiveBarrel barrel = tnt.GetComponent<ExplosiveBarrel>();
                if(barrel)
                {
                    barrel.m_stats.m_timerLength = 3.0f;
                    barrel.m_stats.m_explosiveRadius = 0.5f;
                    barrel.TakeDamage(1);
                }
                //Invoke()
            }
        }
    }
}
