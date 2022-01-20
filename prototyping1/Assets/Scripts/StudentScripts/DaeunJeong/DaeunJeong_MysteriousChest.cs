using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaeunJeong_MysteriousChest : MonoBehaviour
{
    // If you want to spawn only monster from this chest, add only one in the list.
    // Then only that one will be spawned from this chest.
    [Header("Please add perfabs/gameobjects you want to put in this chest.")]
    public List<GameObject> ThingsCanGetFromChest;
    public GameObject EffectAnimation;
    private bool isTriggered = false;

    private void Start()
    {
        for (int i = ThingsCanGetFromChest.Count - 1; i > -1; --i)
        {
            if (ThingsCanGetFromChest[i] == null)
            {
                ThingsCanGetFromChest.RemoveAt(i);
            }
        }
    }

    IEnumerator ChestOpenCoroutine(bool shouldSpawn)
    {
        GameObject effect = Instantiate(EffectAnimation, transform.position, transform.rotation);
        Destroy(effect, 0.5f);

        transform.GetChild(0).gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        if (shouldSpawn)
        {
            Instantiate(ThingsCanGetFromChest[Random.Range(0, ThingsCanGetFromChest.Count)], transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isTriggered)
            {
                // I don't know why but GetKeyDown does not work sometimes...
                // So I used GetKey and isTriggered.
                if (Input.GetKey(KeyCode.E))
                {
                    isTriggered = true;

                    if (ThingsCanGetFromChest.Count > 0)
                    {
                        StartCoroutine(ChestOpenCoroutine(true));
                    }
                    else
                    {
                        Debug.Log("[DaeunJeong_MysteriousChest] You need to add prefabs/gameobjects in the list.");

                        StartCoroutine(ChestOpenCoroutine(false));
                    }
                }
            }
        }
    }
}