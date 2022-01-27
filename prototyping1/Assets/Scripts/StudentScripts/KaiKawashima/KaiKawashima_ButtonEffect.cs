using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaiKawashima_ButtonEffect : MonoBehaviour
{
    public Image image;
    private bool cycleColor = false;
    private bool increaseGreen = true;
    private Color currentColor;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        currentColor = originalColor = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (cycleColor)
        {
            if (increaseGreen)
            {
                currentColor.g += 0.03f;
                if (currentColor.g >= 1)
                {
                    increaseGreen = false;
                }
            }
            else
            {
                currentColor.g -= 0.03f;
                if (currentColor.g <= 0)
                {
                    increaseGreen = true;
                }
            }

            image.color = currentColor;
        }
    }

    public void ActivateColorCycle()
    {
        cycleColor = true;
        currentColor = image.color;
    }

    public void DeactivateColorCycle()
    {
        cycleColor = false;
        image.color = originalColor;
    }
}
