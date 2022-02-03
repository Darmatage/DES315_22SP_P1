using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenThompson_DestroyTurnWall : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        foreach(Transform child in children)
        {
            if (child == transform)
                continue;

            if(child.gameObject.activeSelf == true && child.gameObject.name != "MinimapDoor")
            {
                return;
            }
        }

        gameObject.SetActive(false);
    }
}
