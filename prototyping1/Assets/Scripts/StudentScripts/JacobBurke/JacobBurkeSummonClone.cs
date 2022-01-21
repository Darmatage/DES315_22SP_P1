using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacobBurkeSummonClone : MonoBehaviour
{
    [SerializeField]
    GameObject playerClone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (GameObject.FindGameObjectWithTag("JacobBurkePlayerClone") == null) && playerClone)
        {
            Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
            pos.x += 3;
            Instantiate(playerClone, pos, Quaternion.identity);
        }
    }
}
