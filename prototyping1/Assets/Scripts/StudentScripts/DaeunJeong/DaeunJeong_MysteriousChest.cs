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
    public GameObject Canvas;
    public GameObject UIPanelPrefab;
    public DaeunJeong_UIManager UIManager;
    public bool shouldSpawnSwitch;
    
    private bool isDestroyed = false;

    private void Start()
    {
        bool isThereUIPanel = false;

        if (UIPanelPrefab != null && Canvas != null)
        {
            for (int i = 0; i < Canvas.gameObject.transform.childCount; ++i)
            {
                if (Canvas.gameObject.transform.GetChild(i).gameObject.GetComponent<DaeunJeong_UIManager>())
                {
                    isThereUIPanel = true;
                    UIManager = Canvas.gameObject.transform.GetChild(i).gameObject.GetComponent<DaeunJeong_UIManager>();
                }
            }

            if (!isThereUIPanel)
            {
                GameObject UIPanel = Instantiate(UIPanelPrefab, Canvas.transform);
                UIManager = UIPanel.GetComponent<DaeunJeong_UIManager>();
            }
        }
        else
        {
            Debug.Log("[DaeunJeong_MysteriousChest] You need to assign UI Panel prefab and Canvas.");
        }

        for (int i = ThingsCanGetFromChest.Count - 1; i > -1; --i)
        {
            if (ThingsCanGetFromChest[i] == null || ThingsCanGetFromChest[i].name == "Switch")
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
            GameObject objectToSpawn = ThingsCanGetFromChest[Random.Range(0, ThingsCanGetFromChest.Count)];
            Instantiate(objectToSpawn, transform.position, transform.rotation);
            UIManager.ShowUIText(objectToSpawn.name);
        }

        UIManager.isTriggered = false;
        isDestroyed = true;
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!UIManager.everTriggered)
            {
                UIManager.ShowKeyInstruction();
            }

            if (!UIManager.isTriggered)
            {
                // I don't know why but GetKeyDown does not work sometimes...
                // So I used GetKey and isTriggered.
                if (Input.GetKey(KeyCode.E))
                {
                    UIManager.isTriggered = true;

                    if (!UIManager.everTriggered)
                    {
                        UIManager.everTriggered = true;
                        UIManager.StopShowingUIPanel();
                    }

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isDestroyed)
            {
                UIManager.StopShowingUIPanel();
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}