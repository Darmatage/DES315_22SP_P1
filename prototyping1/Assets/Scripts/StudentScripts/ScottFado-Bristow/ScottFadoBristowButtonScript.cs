using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScottFadoBristowButtonScript : MonoBehaviour
{
    public float HueTime = 5.0f;

    List<(float, float, float)> rainbow;
    float timer = 0;
    int cIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        rainbow = new List<(float, float, float)>();

        //Lets add the colors
        //RED
        rainbow.Add((1, 0, 0));
        //MAGENTA
        rainbow.Add((1, 0, 1));
        //BLUE
        rainbow.Add((0, 0, 1));
        //TURQOISE
        rainbow.Add((0, 1, 1));
        //GREEN
        rainbow.Add((0, 1, 0));
        //YELLOW
        rainbow.Add((1, 1, 0));

        //Then we loop
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime/HueTime;

       

        float r = Mathf.Lerp(rainbow[cIndex].Item1, rainbow[(cIndex + 1) % rainbow.Count].Item1, timer); ;
        float g = Mathf.Lerp(rainbow[cIndex].Item2, rainbow[(cIndex + 1) % rainbow.Count].Item2, timer); ;
        float b = Mathf.Lerp(rainbow[cIndex].Item3, rainbow[(cIndex + 1) % rainbow.Count].Item3, timer); ;
        Image button = gameObject.GetComponent<Image>();
        button.color = new Color(r, g, b);

        if(timer >= HueTime)
        {
            timer = 0;
            cIndex = (cIndex + 1) % rainbow.Count;
        }
    }
}
