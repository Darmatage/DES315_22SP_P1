using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenThompson_DockParkingIndicator : MonoBehaviour
{
    [SerializeField]
    float TimePerPulseCycle = 5.0f;

    private float pulseTimer = 0.0f;

    private Color originalColor;

    [SerializeField]
    Color fullPulse;

    private SpriteRenderer sp;

    private bool LerpingToFullPulse = true;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        if(sp)
        {
            originalColor = sp.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(sp)
        {
            // If we are lerping to full pulse
            if(LerpingToFullPulse)
            {
                sp.color = Color.Lerp(originalColor, fullPulse, pulseTimer / TimePerPulseCycle);

                if(pulseTimer >= TimePerPulseCycle)
                {
                    LerpingToFullPulse = false;
                    pulseTimer = 0.0f;
                }
            }
            else
            {
                sp.color = Color.Lerp(fullPulse, originalColor, pulseTimer / TimePerPulseCycle);

                if (pulseTimer >= TimePerPulseCycle)
                {
                    LerpingToFullPulse = true;
                    pulseTimer = 0.0f;
                }
            }

            // Increase the timer
            pulseTimer += Time.deltaTime;
        }
    }
}
