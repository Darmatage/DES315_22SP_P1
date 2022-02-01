using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class B03_UIHandler : MonoBehaviour
{
    [SerializeField] B03_Trigger UITrigger = null;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(UITrigger, "Please set the trigger for the UI handler.");

        UITrigger.OnEnter = ShowUI;
        UITrigger.OnExit = HideUI;
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
