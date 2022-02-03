using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErinScribner_Particles : MonoBehaviour
{
    public GameObject particles;
    private Transform playerTrans;
    private GameObject newparticles;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
             Vector3 playerFeet = new Vector3(playerTrans.position.x, playerTrans.position.y - .8f, playerTrans.position.z);
             newparticles = Instantiate(particles, playerFeet, Quaternion.identity);
        }
        
        if(newparticles != null)
        {
            Destroy(newparticles, 2);
        }

    }
}
