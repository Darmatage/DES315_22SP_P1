using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JasonHunt_HammerScript : MonoBehaviour
{
    public float HammerRange = 1;
    public GameObject player;
    public GameObject uiElement;
    public bool SelfDestruct = false;
    private float timer = 2;
    bool hasHammer = false;
    GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        blocks = GameObject.FindGameObjectsWithTag("JasonHuntBreakable");

        uiElement.SetActive(false);

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (hasHammer)
        {
            uiElement.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && hasHammer && !SelfDestruct)
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
                    block.GetComponent<JasonHunt_BlockScript>().explode();
                    Destroy(block);
                }
            }

            blocks = GameObject.FindGameObjectsWithTag("JasonHuntBreakable");
        }
        else if (SelfDestruct)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                Destroy(this.gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !SelfDestruct)
        {
            //if collided with player, set hasHammer to true, make opacity 0
            hasHammer = true;
            Destroy(GameObject.Find("JasonHunt_HammerArt"));
        }
    }
}
