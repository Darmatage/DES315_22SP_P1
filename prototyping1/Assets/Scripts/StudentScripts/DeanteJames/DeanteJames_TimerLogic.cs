using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimerLogic : MonoBehaviour
{
    public Text timerText;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timeElasped = (Time.timeSinceLevelLoad);
        string minutes = "";
        string seconds = "";
        string milliSeconds = "";
        if (!((int)timeElasped % 60 > 9))
        {
            seconds = "0";
        }

        if (!((int)timeElasped / 60 > 9))
        {
            minutes = "0";
        }

        minutes += ((int)timeElasped / 60).ToString();
        seconds += ((int)timeElasped % 60).ToString();
        milliSeconds = Mathf.RoundToInt((timeElasped * 100) % 100).ToString();


        timerText.text = minutes + ":" + seconds + ":" + milliSeconds;
    }
}
