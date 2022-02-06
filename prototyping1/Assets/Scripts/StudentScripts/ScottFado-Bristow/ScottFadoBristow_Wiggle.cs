using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScottFadoBristow_Wiggle : MonoBehaviour
{

    public float WiggleTime = 0.5f;
    public float timer;
    public bool Wiggle = false;
    public float WiggleOff = 1.0f;

    private Vector3 startingPos;
    private Vector3 wigglePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Wiggle)
        {
            //Perfrom said wiggle
            //what the hell do I do here
            transform.localPosition = Vector3.Lerp(startingPos, wigglePos, timer /WiggleTime);

            timer -= Time.deltaTime;

            if (timer <= 0)
                Wiggle = false;
        }
    }


    public void StartWiggle()
    {
        Wiggle = true;
        timer = WiggleTime;
        startingPos = transform.localPosition;
        wigglePos = new Vector3(Random.Range(-WiggleOff, WiggleOff), Random.Range(-WiggleOff, WiggleOff), transform.localPosition.z);
    }
}
