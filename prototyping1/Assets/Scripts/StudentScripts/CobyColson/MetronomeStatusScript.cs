using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetronomeStatusScript : MonoBehaviour
{
    public struct StatusPreset
    {
        public string text;
        public Color col;
        public StatusPreset(string _text, Color _col)
        {
            text = _text;
            col = _col;
        }
    };

    public static StatusPreset[] presets = new StatusPreset[]
    {
        new StatusPreset("PERFECT!", Color.yellow),
        new StatusPreset("GOOD!", Color.cyan),
        new StatusPreset("MISS", Color.red),
        new StatusPreset("", Color.clear),
        new StatusPreset("Get a PERFECT...", Color.black)
    };

    private GameObject textObject;
    Text text;
    RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        textObject = GameObject.FindGameObjectWithTag("CC_StatusText");
        text = textObject.GetComponent<Text>();
        rect = textObject.GetComponent<RectTransform>();
        SetTextToPreset(presets[3]);
    }

    // Update is called once per frame
    void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.00075f);
        rect.anchoredPosition = new Vector2(0.0f, rect.anchoredPosition.y + 0.15f);
        rect.localEulerAngles = new Vector3(rect.localEulerAngles.x, 30 * (Mathf.Sin(Time.time * 5) - 0.5f), rect.localEulerAngles.z);
    }

    public void SetTextToPreset(StatusPreset preset)
    {
        text.text = preset.text;
        text.color = preset.col;
        Reset();
    }

    private void Reset()
    {
        rect.anchoredPosition = new Vector2(0.0f, -210.0f);
        rect.localEulerAngles = new Vector3(0.0f, 0.0f, Random.Range(-20.0f, 20.0f));
    }
}
