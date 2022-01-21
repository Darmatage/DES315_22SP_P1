using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacobBurkeSummonClone : MonoBehaviour
{
    [SerializeField]
    GameObject playerClone;

    private GameObject playerCloneInstance;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (playerCloneInstance == null) && playerClone)
        {
            Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
            pos.x += 3;
            playerCloneInstance = Instantiate(playerClone, pos, Quaternion.identity);
        }
    }
}
