using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeBarScript : MonoBehaviour
{
    enum Direction { Left, Right };
    private Direction direction;
    private RectTransform rect;
    public float barSpeed;
    private float canvasHalfWidth = 640;
    private AudioSource[] audioSources;
    private MetronomeControllerScript controller;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        audioSources = GameObject.Find("MetronomeController").GetComponents<AudioSource>();
        controller = GameObject.Find("MetronomeController").GetComponent<MetronomeControllerScript>();
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
        if (direction == Direction.Left)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x - barSpeed * Time.deltaTime, rect.anchoredPosition.y);
        }
        else
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + barSpeed * Time.deltaTime, rect.anchoredPosition.y);
        }
        if (Mathf.Abs(rect.anchoredPosition.x) < 1)
        {
            if (!audioSources[1].isPlaying)
            {
                audioSources[1].Play(0);
            }
            if (direction == Direction.Left)
            {
                rect.anchoredPosition = new Vector2(canvasHalfWidth, rect.anchoredPosition.y);
            }
            else
            {
                rect.anchoredPosition = new Vector2(-canvasHalfWidth, rect.anchoredPosition.y);
            }
            controller.ResetActions();
        }
    }
}
