//Kiara Santiago
//DES 315
//Basic Hiding Script (used with the caves)
//Spring 22

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_Hide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("Player is in cave.");

            GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            
            foreach (GameObject enemy in Enemies)
            {
                Kiara_AlteredMonsterMoveHit tempScript = enemy.GetComponent<Kiara_AlteredMonsterMoveHit>();
                tempScript.attackPlayer = false;
                tempScript.canSeePlayer = false;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Player exited cave.");

            GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in Enemies)
            {
                Kiara_AlteredMonsterMoveHit tempScript = enemy.GetComponent<Kiara_AlteredMonsterMoveHit>();
                tempScript.attackPlayer = true;
                tempScript.canSeePlayer = true;
            }
        }
    }
}
