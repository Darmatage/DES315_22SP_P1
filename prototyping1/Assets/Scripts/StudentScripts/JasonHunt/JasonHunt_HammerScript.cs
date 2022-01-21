using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JasonHunt_HammerScript : MonoBehaviour
{
    public float HammerRange = 1;
    public GameObject player;
    bool hasHammer = false;
    GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        blocks = GameObject.FindGameObjectsWithTag("JasonHuntBreakable");

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && hasHammer)
        {
            //swing hammer (animation coming eventually)
            //if any objects tagged with "JasonHuntBreakable" are within HammerRange distance of the player, destroy those objects
            foreach (GameObject block in blocks)
            {
                if (!block) continue;
                //if distance between block and player is less than HammerRange
                if (Vector3.Distance(block.GetComponent<Transform>().position, player.GetComponent<Transform>().position) <= HammerRange)
                {
                    //break block
                    Destroy(block);
                }
            }

            blocks = GameObject.FindGameObjectsWithTag("JasonHuntBreakable");
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //if collided with player, set hasHammer to true, make opacity 0
            hasHammer = true;
            Destroy(GameObject.Find("JasonHunt_HammerArt"));
        }
    }
}
