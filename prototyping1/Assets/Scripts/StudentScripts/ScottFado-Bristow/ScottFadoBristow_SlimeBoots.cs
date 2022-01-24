using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScottFadoBristow_SlimeBoots : MonoBehaviour
{

    public GameObject AttatchSlime;
    public Vector2 BootsOffset;
    public Vector2 SlimeOffset;
    private Stack<(GameObject, float)> slimes;
    private float wiggleTimer = 4;
    private bool wiggleDir = false;
    private int wiggleCount = 0;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        slimes = new Stack<(GameObject, float)>();
    }

    // Update is called once per frame
    void Update()
    {
        //Alright!! Wiggle check
        //Check if player is moving left to right
        float input = Input.GetAxisRaw("Horizontal");
        if(input > 0)
        {
            if(wiggleDir == false)
            {
                wiggleCount++;
                wiggleDir = true;
            }
        }
        else if (input < 0)
        {
            if (wiggleDir == true)
            {
                wiggleCount++;
                wiggleDir = false;
         
            }
        }

        if(wiggleCount >= 10)
        {
            Detach();
            wiggleCount = 0;
        }
        
    }



    public void Attatch(GameObject p, float speedDiff)
    {
        //Create the new child and attatch it to this object
        GameObject newSlime = Instantiate(AttatchSlime, transform, false);
        float x = BootsOffset.x + Random.Range(-SlimeOffset.x, SlimeOffset.x);
        float y = BootsOffset.y + Random.Range(-SlimeOffset.y, SlimeOffset.y);
        newSlime.transform.localPosition = new Vector3(x, y, newSlime.transform.position.z);
        slimes.Push((newSlime, speedDiff));

        player = p;

    }


    public void Detach()
    {
        if (slimes.Count > 0)
        {
            (GameObject, float ) p = slimes.Pop();
            GameObject s = p.Item1;
            s.GetComponent<ScottFadoBristow_BootSlimeControl>().Kill();
            player.GetComponent<PlayerMove>().speed += p.Item2;
        }
    }

}
