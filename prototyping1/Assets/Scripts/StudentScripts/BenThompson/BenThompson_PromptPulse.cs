using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenThompson_PromptPulse : MonoBehaviour
{
    // The minimum scale value that will be used for the prompt size
    [SerializeField]
    float minUniformScale = 3.0f;

    // The maximum scale value that will be used for the prompt size
    [SerializeField]
    float maxUniformScale = 5.0f;

    // The speed at which the pulse occurs
    [SerializeField]
    float pulseSpeed = 0.05f;

    [SerializeField]
    float maxPulsePauseTime = 2.0f;

    private float pauseTime = 0.0f;

    // The current uniform scale value being used
    private float uniformScale = 3.0f;

    [SerializeField]
    private SpriteRenderer promptRenderer;

    // Booleans to indicate which state we are in
    private bool growing = true;
    private bool shrinking = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.localScale = new Vector3(1 / transform.parent.lossyScale.x, 1 / transform.parent.lossyScale.y, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(promptRenderer)
        {
            if (promptRenderer.enabled == false)
                return;
        }

        if(pauseTime > 0.0f)
        {
            pauseTime -= Time.deltaTime;
            return;
        }

        // If we are growing
        if(growing)
        {
            // If we have not grown all the way yet
            if(uniformScale < maxUniformScale)
            {
                uniformScale = Mathf.MoveTowards(uniformScale, maxUniformScale, pulseSpeed);
            }
            else
            {
                growing = false;
                shrinking = true;
                pauseTime = maxPulsePauseTime;
            }
        }

        // If we are shrinking
        else if(shrinking)
        {
            // If we have not shrunk all the way yet
            if (uniformScale > minUniformScale)
            {
                uniformScale = Mathf.MoveTowards(uniformScale, minUniformScale, pulseSpeed);
            }
            else
            {
                growing = true;
                shrinking = false;
            }
        }

        // Scale the prompt
        transform.localScale = new Vector3(uniformScale, uniformScale, 1);
    }
}
