using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the purpose of this script is to have a walkable block be placed on a block of lava that the
//player character is looking at
public class ErinScribner_IcePowers : MonoBehaviour
{
    private GameHandler gameHandlerObj;
    private Transform playerTrans;
    public GameObject iceBlock;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("GameHandler") != null)
        {
            gameHandlerObj = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        }
        playerTrans = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
           // GameObject iceblock = GameObject.Find("IceBlock");
            Vector3 playerFeet = new Vector3(playerTrans.position.x, playerTrans.position.y - .8f, playerTrans.position.z);
            GameObject iceBlockNew = Instantiate(iceBlock, playerFeet, Quaternion.identity);

            Vector3 playerFeet2 = new Vector3(playerTrans.position.x - 1.0f, playerTrans.position.y, playerTrans.position.z);
            GameObject iceBlockNew2 = Instantiate(iceBlock, playerFeet2, Quaternion.identity);

            Vector3 playerFeet3 = new Vector3(playerTrans.position.x + 1.0f, playerTrans.position.y, playerTrans.position.z);
            GameObject iceBlockNew3 = Instantiate(iceBlock, playerFeet3, Quaternion.identity);

            Vector3 playerFeet4 = new Vector3(playerTrans.position.x, playerTrans.position.y + 1, playerTrans.position.z);
            GameObject iceBlockNew4 = Instantiate(iceBlock, playerFeet4, Quaternion.identity);

            Vector3 playerFeet5 = new Vector3(playerTrans.position.x, playerTrans.position.y, playerTrans.position.z);
            GameObject iceBlockNew5 = Instantiate(iceBlock, playerFeet5, Quaternion.identity);

            // GameObject player = GameObject.Find("Player");

            //iceblock.transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y - .8f, playerTrans.position.z);
        }
    } 

}
