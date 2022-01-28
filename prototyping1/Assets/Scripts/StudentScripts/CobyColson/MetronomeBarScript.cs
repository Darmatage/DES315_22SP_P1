using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeBarScript : MonoBehaviour
{
    enum Direction { Left, Right };
    private Direction direction;
    private RectTransform rect;
    public static float barSpeed = 300;
    public static float initialBarSpeed;
    private float canvasHalfWidth = 640;
    private AudioSource[] audioSources;
    private MetronomeControllerScript controller;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        audioSources = GameObject.Find("MetronomeController").GetComponents<AudioSource>();
        controller = GameObject.Find("MetronomeController").GetComponent<MetronomeControllerScript>();
        initialBarSpeed = barSpeed;
        if (rect.anchoredPosition.x < 0)
        {
            direction = Direction.Right;
        }
        else
        {
            direction = Direction.Left;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool resetPosition = false;
        if (direction == Direction.Left)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x - barSpeed * Time.deltaTime, rect.anchoredPosition.y);
            if (rect.anchoredPosition.x < 0)
            {
                resetPosition = true;
                rect.anchoredPosition = new Vector2(canvasHalfWidth, rect.anchoredPosition.y);
            }
        }
        else
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + barSpeed * Time.deltaTime, rect.anchoredPosition.y);
            if (rect.anchoredPosition.x > 0)
            {
                resetPosition = true;
                rect.anchoredPosition = new Vector2(-canvasHalfWidth, rect.anchoredPosition.y);
            }
        }
        if (resetPosition)
        {
            if (!audioSources[1].isPlaying)
            {
                audioSources[1].Play(0);
            }
            controller.ResetActions();
        }
    }
}
