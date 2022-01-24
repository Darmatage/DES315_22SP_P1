using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaeunJeong_ChestManager : MonoBehaviour
{
    public GameObject switchPrefab;

    private GameObject[] Chests;
    private GameObject switchChest;

    void Start()
    {
        Chests = GameObject.FindGameObjectsWithTag("MysteriousChest");

        if (Chests.Length >= 1)
        {
            SetSwitchChest();
        }
    }

    void SetSwitchChest()
    {
        for (int i = 0; i < Chests.Length; ++i)
        {
            if (Chests[i].GetComponent<DaeunJeong_MysteriousChest>().shouldSpawnSwitch)
            {
                switchChest = Chests[Random.Range(0, Chests.Length)];
                switchChest.GetComponent<DaeunJeong_MysteriousChest>().ThingsCanGetFromChest.Clear();
                switchChest.GetComponent<DaeunJeong_MysteriousChest>().ThingsCanGetFromChest.Add(switchPrefab);
                return;
            }
        }
    }
}
