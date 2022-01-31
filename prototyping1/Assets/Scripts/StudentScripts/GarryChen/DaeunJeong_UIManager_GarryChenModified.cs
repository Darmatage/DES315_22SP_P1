using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaeunJeong_UIManager_GarryChenModified : MonoBehaviour
{
    public bool isTriggered;
    public bool everTriggered;

    private GameObject text;
    private Image imageComponent;

    private bool isSpawned;
    private bool isStopping;
    private float timer = 0.0f;
    private float lifeTime = 2.0f;

    void Start()
    {
        isTriggered = false;
        everTriggered = false;
        isStopping = false;

        imageComponent = GetComponent<Image>();
        imageComponent.enabled = false;

        text = gameObject.transform.GetChild(0).gameObject;
        text.SetActive(false);
    }

    private void Update()
    {
        if (isSpawned && isStopping)
        {
            timer += Time.deltaTime;

            if (timer >= lifeTime)
            {
                StopShowingUIPanel();
            }
        }
    }

    public void SetStopShowingUIPanel(float _lifeTime)
    {
        lifeTime = _lifeTime;
        isStopping = true;
        timer = 0.0f;
    }

    public void StopShowingUIPanel()
    {
        imageComponent.enabled = false;
        isSpawned = false;
        text.SetActive(false);
    }

    public void ShowUIText(string s)
    {
        imageComponent.enabled = true;
        text.GetComponent<Text>().text = s;
        text.GetComponent<Text>().text += "\n(UI by DaeunJeong)";
        text.SetActive(true);
        isSpawned = true;
        isStopping = false;
    }

}
