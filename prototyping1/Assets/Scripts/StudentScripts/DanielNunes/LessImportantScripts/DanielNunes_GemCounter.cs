using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DanielNunes_GemCounter : MonoBehaviour
{
    public int totalGems = 3;
    public int gemsFound = 0;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = transform.Find("Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText()
    {
        text.text = gemsFound.ToString() + "/" + totalGems.ToString();
    }
}
