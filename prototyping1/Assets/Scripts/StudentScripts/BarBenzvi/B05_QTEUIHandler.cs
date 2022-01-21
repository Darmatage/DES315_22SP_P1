using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class B05_QTEUIButtonData
{
    public GameObject ParentObject = null;
    public GameObject BackgroundObject = null;
    public GameObject DisplayObject = null;
}


[System.Serializable]
public class B05_QTEComboButtonData
{
    public KeyCode KeyToPress = KeyCode.Space;
    public Color KeyColor = Color.red;
    //This is temporary - currently using this just to avoid making 4 arrow sprites that just face different directions
    public float KeyAngle = 0.0f;
}


public class B05_QTEUIHandler : MonoBehaviour
{
    public float TimeToComplete = 5.0f;

    public RectTransform TimerBar = null;

    public List<B05_QTEUIButtonData> KeyButtonDisplays = new List<B05_QTEUIButtonData>();
    public List<B05_QTEComboButtonData> PossibleComboButtons = new List<B05_QTEComboButtonData>();

    public Color WrongColor = Color.red;
    public Color CorrectColor = Color.green;
    public Vector3 CorrectFlyoffVelocity = Vector3.up * 100.0f;

    int currIndex = 0;
    List<KeyCode> keyCombo = new List<KeyCode>();
    float timer = 0.0f;
    float maxTimerScale = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        keyCombo.Clear();
        if(TimerBar != null)
        {
            maxTimerScale = TimerBar.rect.width;
        }

        //Generate a key combo that is the same length as our dispalys
        foreach(B05_QTEUIButtonData button in KeyButtonDisplays)
        {
            B05_QTEComboButtonData comboButton = PossibleComboButtons[Random.Range(0, PossibleComboButtons.Count)];
            keyCombo.Add(comboButton.KeyToPress);

            button.DisplayObject.GetComponent<Image>().color = comboButton.KeyColor;
            button.DisplayObject.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, comboButton.KeyAngle);
        }

        timer = TimeToComplete;
    }

    // Update is called once per frame
    void Update()
    {
        //If any key was pressed, we want to check if it was the right one
        if(Input.anyKeyDown)
        {
            if(Input.GetKeyDown(keyCombo[currIndex]))
            {
                GameObject bg = KeyButtonDisplays[currIndex].BackgroundObject;
                GameObject bParent = KeyButtonDisplays[currIndex].ParentObject;
                bg.GetComponent<B05_ColorInterpolator>().TargetColor = CorrectColor;
                bParent.GetComponent<B05_ConstantTransformMovement>().Velocity = CorrectFlyoffVelocity;

                ++currIndex;
                if(currIndex == keyCombo.Count)
                {
                    Time.timeScale = 1.0f;
                    B05_EventManager.CallQTESuccess();
                    B05_EventManager.CallQTEEnded();
                    Destroy(gameObject);
                }
            }
            else
            {
                GameObject bg = KeyButtonDisplays[currIndex].BackgroundObject;
                bg.GetComponent<Image>().color = WrongColor;
            }
        }

        timer -= Time.unscaledDeltaTime;

        if(TimerBar != null)
        {
            TimerBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, maxTimerScale, timer / TimeToComplete));
            TimerBar.GetComponent<Image>().color = Color.Lerp(WrongColor, CorrectColor, timer / TimeToComplete);
        }

        if(timer <= 0.0f)
        {
            Time.timeScale = 1.0f;
            B05_EventManager.CallQTEFailure();
            B05_EventManager.CallQTEEnded();
            Destroy(gameObject);
        }
    }
}
