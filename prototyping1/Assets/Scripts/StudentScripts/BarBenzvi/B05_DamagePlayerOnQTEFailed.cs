using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_DamagePlayerOnQTEFailed : MonoBehaviour
{
    public GameHandler HandlerObj = null;

    public int DamageOnFail = 10;

    private void OnEnable()
    {
        B05_EventManager.OnQTEFailure += OnQTEFailure;
    }

    private void OnDisable()
    {
        B05_EventManager.OnQTEFailure -= OnQTEFailure;
    }

    void OnQTEFailure()
    {
        if(HandlerObj != null)
        {
            HandlerObj.TakeDamage(DamageOnFail);
        }
    }
}
