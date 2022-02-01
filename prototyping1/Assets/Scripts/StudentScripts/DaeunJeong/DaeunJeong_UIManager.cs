using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaeunJeong_UIManager : MonoBehaviour
{
    public bool isTriggered;
    public bool everTriggered;

    private GameObject text;
    private Image imageComponent;

    private bool isSpawned;
    private float timer = 0.0f;
    private readonly float lifeTime = 2.0f;

    void Start()
    {
        isTriggered = false;
        everTriggered = false;

        imageComponent = GetComponent<Image>();
        imageComponent.enabled = false;

        text = gameObject.transform.GetChild(0).gameObject;
        text.SetActive(false);
    }

    private void Update()
    {
        if (isSpawned)
        {
            timer += Time.deltaTime;

            if (timer >= lifeTime)
            {
                StopShowingUIPanel();
                timer = 0.0f;
                isSpawned = false;
            }
        }
    }

    public void ShowKeyInstruction()
    {
        imageComponent.enabled = true;
        text.SetActive(true);
    }

    public void StopShowingUIPanel()
    {
        imageComponent.enabled = false;
        text.SetActive(false);
    }

    public void ShowUIText(string objectName)
    {
        imageComponent.enabled = true;
        text.GetComponent<Text>().text = "A wild ";
        text.GetComponent<Text>().text += objectName;
        text.GetComponent<Text>().text += " appeared!";
        text.SetActive(true);

        isSpawned = true;
    }

    public void ShowUITextForPickups(DaeunJeong_Pickups.PICKUP_TYPE pickupType)
    {
        switch (pickupType)
        {
            case DaeunJeong_Pickups.PICKUP_TYPE.COIN:
                text.GetComponent<Text>().text = "You got a coin! Every penny counts...";
                break;
            case DaeunJeong_Pickups.PICKUP_TYPE.HEALTH:
                text.GetComponent<Text>().text = "You feel better now!";
                break;
            case DaeunJeong_Pickups.PICKUP_TYPE.JEWEL:
                text.GetComponent<Text>().text = "You got a diamond! You are rich!";
                break;
        }

        imageComponent.enabled = true;
        text.SetActive(true);

        isSpawned = true;
        timer = 0.0f;
    }
}
