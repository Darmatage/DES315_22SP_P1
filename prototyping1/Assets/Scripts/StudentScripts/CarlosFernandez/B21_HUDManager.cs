using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B21_HUDManager : MonoBehaviour
{
    private Canvas canvas;

    private GameObject text;
    private Text newText;
    private int cdNumber;
    private B21_ProjectileTeleport scriptReference;
    public Color textColor;
    
    // Start is called before the first frame update
    void Start()
    {
        scriptReference = gameObject.GetComponent<B21_ProjectileTeleport>();
        cdNumber = (int)scriptReference.cooldownTimer;
        GameObject g = GameObject.Find("Canvas");
        canvas = g.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        text = new GameObject("UIText", typeof(RectTransform))
        {
            transform =
            {
                parent = canvas.transform
            },
            layer = 5
        };
        text.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        text.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500.0f, 0, 0);
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(200.0f, 100.0f);

        newText = text.AddComponent<Text>();
        newText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        newText.fontSize = 23;
        newText.color = textColor;
    }

    private void PrintText()
    {
        newText.text = "Teleport : " + scriptReference.shootKeybind.ToString() + "\n"
                       + "Cancel Teleport: " + scriptReference.cancelShootKeybind.ToString() + "\n";
        if (scriptReference.cooldownTimer > 0.0f)
        {
            newText.text += "Cooldown: " + cdNumber + "\n";

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (scriptReference.cooldownTimer > 0.0f)
        {
            cdNumber = (int)scriptReference.cooldownTimer;
        }
        PrintText();
    }

    private void FixedUpdate()
    {
        newText.text = "";
        
    }
}
