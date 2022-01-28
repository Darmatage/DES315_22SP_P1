using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScottFadoBristowButtonScript : MonoBehaviour
{
    public float HueTime = 5.0f;

    //public List<(float, float, float)> rainbow;

    public Color[] rainbow;

    float timer = 0;
    int cIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        /*
        if(rainbow == null)
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
        */

        //Then we loop
    }

    // Update is called once per frame
    void Update()
    {



        timer += Time.deltaTime/HueTime;

        if (rainbow.Length > 0)
        {

            float r = Mathf.Lerp(rainbow[cIndex].r, rainbow[(cIndex + 1) % rainbow.Length].r, timer);
            float g = Mathf.Lerp(rainbow[cIndex].g, rainbow[(cIndex + 1) % rainbow.Length].g, timer);
            float b = Mathf.Lerp(rainbow[cIndex].b, rainbow[(cIndex + 1) % rainbow.Length].b, timer);
            Image button = gameObject.GetComponent<Image>();
            if (button != null)
            {
                button.color = new Color(r, g, b);
            }
            Text text = gameObject.GetComponent<Text>();
            if (text != null)
            {
                text.color = new Color(r, g, b);
            }

            if (timer >= HueTime)
            {
                timer = 0;
                cIndex = (cIndex + 1) % rainbow.Length;
            }
        }
    }
}
