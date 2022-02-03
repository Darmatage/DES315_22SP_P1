using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenThompson_MinimapSnap : MonoBehaviour
{
    GameObject player;
    Vector3 startPos;

    [SerializeField]
    float minimapSize;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(player.transform.position, startPos));

        if (Vector3.Distance(player.transform.position, startPos) < minimapSize)
        {
            transform.position = startPos;
        }
        else
        {
            transform.position = player.transform.position + Vector3.Normalize(startPos - player.transform.position) * minimapSize;
        }
    }
}
