using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErinScribner_Particles : MonoBehaviour
{
    public GameObject particles;
    private Transform playerTrans;
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
             particles = Instantiate(particles, playerFeet, Quaternion.identity);
        }
    }
}
