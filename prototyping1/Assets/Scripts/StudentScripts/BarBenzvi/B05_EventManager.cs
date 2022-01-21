using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_EventManager : MonoBehaviour
{
    public delegate void QTEAction();
    public static event QTEAction OnQTEStarted;
    public static event QTEAction OnQTESuccess;
    public static event QTEAction OnQTEFailure;
    public static event QTEAction OnQTEEnded;

    public static void CallQTEStarted()
    {
        if(OnQTEStarted != null)
        {
            OnQTEStarted();
        }
    }

    public static void CallQTESuccess()
    {
        if (OnQTESuccess != null)
        {
            OnQTESuccess();
        }
    }

    public static void CallQTEFailure()
    {
        if (OnQTEFailure != null)
        {
            OnQTEFailure();
        }
    }

    public static void CallQTEEnded()
    {
        if (OnQTEEnded != null)
        {
            OnQTEEnded();
        }
    }
}
