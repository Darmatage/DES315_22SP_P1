using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaeunJeong_ChestManager : MonoBehaviour
{
    public GameObject switchPrefab;

    private GameObject[] Chests;
    private GameObject switchChest;
    private GameObject[] switchChests;

    void Start()
    {
        Chests = GameObject.FindGameObjectsWithTag("MysteriousChest");

        if (Chests.Length >= 1)
        {
            SetSwitchChest();
        }
        else if (Chests.Length >= 10)
        {
            SetMultipleSwitchChest();
        }
    }

    void SetSwitchChest()
    {
        for (int i = 0; i < Chests.Length; ++i)
        {
            if (Chests[i].GetComponent<DaeunJeong_MysteriousChest>().shouldSpawnSwitch)
            {
                GameObject switchObject = GameObject.Find("Switch");

                if (switchObject != null)
                {
                    if (switchObject.GetComponent<DoorSwitch>() != null)
                    {
                        Destroy(switchObject);
                    }
                }

                switchChest = Chests[Random.Range(0, Chests.Length)];
                switchChest.GetComponent<DaeunJeong_MysteriousChest>().ThingsCanGetFromChest.Clear();
                switchChest.GetComponent<DaeunJeong_MysteriousChest>().ThingsCanGetFromChest.Add(switchPrefab);
                return;
            }
        }
    }

    void SetMultipleSwitchChest()
    {
        for (int i = 0; i < Chests.Length; ++i)
        {
            if (Chests[i].GetComponent<DaeunJeong_MysteriousChest>().shouldSpawnSwitch)
            {
                GameObject switchObject = GameObject.Find("Switch");

                if (switchObject != null)
                {
                    if (switchObject.GetComponent<DoorSwitch>() != null)
                    {
                        Destroy(switchObject);
                    }
                }

                for (int j = 0; j < Chests.Length / 2; ++j)
                {
                    switchChests[j] = Chests[Random.Range(0, Chests.Length)];
                    switchChests[j].GetComponent<DaeunJeong_MysteriousChest>().ThingsCanGetFromChest.Clear();
                    switchChests[j].GetComponent<DaeunJeong_MysteriousChest>().ThingsCanGetFromChest.Add(switchPrefab);
                }

                return;
            }
        }
    }
}
