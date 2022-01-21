using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B05_ColorInterpolator : MonoBehaviour
{
    public Color TargetColor = Color.white;
    //This is multiplied by delta time
    public float LerpSpeed = 0.8f;

    Image img = null;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(img != null)
        {
            Color lerped = Color.Lerp(img.color, TargetColor, LerpSpeed * Time.unscaledDeltaTime);
            img.color = lerped;
        }
    }
}
