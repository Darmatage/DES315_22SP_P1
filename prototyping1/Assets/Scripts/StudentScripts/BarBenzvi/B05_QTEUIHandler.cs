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
    public AudioClip SuccessSFX = null;
}


[System.Serializable]
public class B05_QTEComboButtonData
{
    public List<KeyCode> KeysToPress = new List<KeyCode>();
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

    public int IncorrectKeyDamage = 5;

    public RectTransform PlayerFireball = null;
    public RectTransform PlayerIcon = null;
    public RectTransform EnemyFireball = null;
    public RectTransform EnemyIcon = null;

    public float MinFireballSize = 20;
    public float MaxFireballSize = 100;
    public float FireballFlyTime = 0.3f;

    public AudioClip WrongKeySFX = null;
    public AudioClip FireballImpactSFX = null;

    Vector2 playerFireballPos = new Vector2();
    Vector2 enemyFireballPos = new Vector2();

    int currIndex = 0;
    List<B05_QTEComboButtonData> keyCombo = new List<B05_QTEComboButtonData>();
    float timer = 0.0f;
    float maxTimerScale = 0.0f;
    float fireballTimer = 0.0f;
    bool succeeded = false;

    GameHandler gHandler = null;
    AudioSource aSource = null;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
        if (gameHandlerLocation != null)
        {
            gHandler = gameHandlerLocation.GetComponent<GameHandler>();
        }

        GameObject playerObj = GameObject.FindWithTag("Player");
        if(playerObj != null)
        {
            aSource = playerObj.GetComponent<AudioSource>();
        }
        

        keyCombo.Clear();
        if(TimerBar != null)
        {
            maxTimerScale = TimerBar.rect.width;
        }

        //Generate a key combo that is the same length as our dispalys
        foreach(B05_QTEUIButtonData button in KeyButtonDisplays)
        {
            B05_QTEComboButtonData comboButton = PossibleComboButtons[Random.Range(0, PossibleComboButtons.Count)];
            keyCombo.Add(comboButton);

            button.DisplayObject.GetComponent<Image>().color = comboButton.KeyColor;
            button.DisplayObject.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, comboButton.KeyAngle);
        }

        timer = TimeToComplete;
        fireballTimer = 0.0f;
        if(PlayerFireball && EnemyFireball)
        {
            playerFireballPos = PlayerFireball.position;
            enemyFireballPos = EnemyFireball.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(succeeded || timer <= 0.0f)
        {
            if(succeeded)
            {
                PlayerFireball.position = Vector2.Lerp(playerFireballPos, EnemyIcon.position, fireballTimer / FireballFlyTime);
            }
            else
            {
                EnemyFireball.position = Vector2.Lerp(enemyFireballPos, PlayerIcon.position, fireballTimer / FireballFlyTime);
            }

            fireballTimer += Time.unscaledDeltaTime;

            if(fireballTimer >= FireballFlyTime)
            {
                Time.timeScale = 1.0f;
                if(succeeded)
                {
                    B05_EventManager.CallQTESuccess();
                }
                else
                {
                    B05_EventManager.CallQTEFailure();
                }
                B05_EventManager.CallQTEEnded();
                if(aSource != null)
                {
                    aSource.Stop();
                    aSource.PlayOneShot(FireballImpactSFX);
                }
                Destroy(gameObject);
            }

            return;
        }

        //If any key was pressed, we want to check if it was the right one
        if (Input.anyKeyDown)
        {
            GameObject bg = KeyButtonDisplays[currIndex].BackgroundObject;
            GameObject bParent = KeyButtonDisplays[currIndex].ParentObject;
            bool wrongKey = true;

            foreach (KeyCode key in keyCombo[currIndex].KeysToPress)
            {
                if (Input.GetKeyDown(key))
                {
                    wrongKey = false;

                    bg.GetComponent<B05_ColorInterpolator>().TargetColor = CorrectColor;
                    bParent.GetComponent<B05_ConstantTransformMovement>().Velocity = CorrectFlyoffVelocity;

                    if (aSource != null)
                    {
                        aSource.PlayOneShot(KeyButtonDisplays[currIndex].SuccessSFX);
                    }

                    ++currIndex;
                    if (currIndex == keyCombo.Count)
                    {
                        succeeded = true;
                        break;
                    }
                }
            }
            if(wrongKey)
            {
                bg.GetComponent<Image>().color = WrongColor;
                if(gHandler != null)
                {
                    gHandler.TakeDamage(IncorrectKeyDamage);
                }
                if(aSource != null)
                {
                    aSource.Stop();
                    aSource.PlayOneShot(WrongKeySFX);
                }
            }
        }
        timer -= Time.unscaledDeltaTime;

        float l = timer / TimeToComplete;

        if (TimerBar != null)
        {
            TimerBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, maxTimerScale, l));
            TimerBar.GetComponent<Image>().color = Color.Lerp(WrongColor, CorrectColor, l);
        }
        if(PlayerFireball != null)
        {
            float scaleVal = Mathf.Lerp(MinFireballSize, MaxFireballSize, (float)currIndex / keyCombo.Count);
            PlayerFireball.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scaleVal);
            PlayerFireball.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scaleVal);
        }
        if(EnemyFireball != null)
        {
            float scaleVal = Mathf.Lerp(MaxFireballSize, MinFireballSize, l);
            EnemyFireball.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scaleVal);
            EnemyFireball.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scaleVal);
        }
    }
}
