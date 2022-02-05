using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// generic switch
public class GenericSwitch : MonoBehaviour
{
    public GameObject SwitchOffArt;
    public GameObject SwitchOnArt;


    // generic event can be assigned to do anything
    public UnityEvent OnSwitchOnEvent;

    // generic event can be assigned to do anything
    public UnityEvent OnSwitchOffEvent;

    public bool toggle = false;

    public List<string> tags = new List<string> { "Player" };

    public bool on = false;

    public bool VisualizeConnections = true;

    public GameObject ConnectionArt;

    private bool on_internal = false;


    void Start()
    {
        on_internal = on;
        bool temp = VisualizeConnections;
        VisualizeConnections = false;
        if (on_internal)
            ArtSwitchOn();
        else
            ArtSwitchOff();
        VisualizeConnections = temp;

        //InvokeRepeating("EmitConnections", 1f, 1f);
    }

    private void ArtSwitchOn()
    {
        SwitchOffArt.SetActive(false);
        SwitchOnArt.SetActive(true);

        EmitConnections();
    }

    private void ArtSwitchOff()
    {
        SwitchOffArt.SetActive(true);
        SwitchOnArt.SetActive(false);

        EmitConnections();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // don't switch off if not desired.
        if (toggle || !on_internal)
        {
            foreach (string t in tags)
            {
                if (other.gameObject.tag == t)
                {
                    on_internal = !on_internal;

                    if (on_internal)
                    {
                        ArtSwitchOn();
                        OnSwitchOnEvent.Invoke();
                    }
                    else
                    {
                        ArtSwitchOff();
                        OnSwitchOffEvent.Invoke();
                    }

                    break;
                }
            }
        }
    }


    private void EmitConnections()
    {
        if (VisualizeConnections)
        {
            if (!on)
            {
                int eventCount = OnSwitchOnEvent.GetPersistentEventCount();
                for (int i = 0; i < eventCount; ++i)
                {
                    Object target = OnSwitchOnEvent.GetPersistentTarget(i);
                    EmitConnector(target);
                }
            }
            else
            {
                int eventCount = OnSwitchOffEvent.GetPersistentEventCount();
                for (int i = 0; i < eventCount; ++i)
                {
                    Object target = OnSwitchOffEvent.GetPersistentTarget(i);
                    EmitConnector(target);
                }
            }
        }
    }

    private void EmitConnector(Object target)
    {
        GameObject connectionArt = Instantiate(ConnectionArt);
        connectionArt.transform.position = transform.position;

        Vector3 targetPosition;
        if (target is GameObject)
            targetPosition = ((GameObject)target).transform.position;
        else if (target is Component)
            targetPosition = ((Component)target).transform.position;
        else
            return;


        Vector3 offset = targetPosition - connectionArt.transform.position;
        connectionArt.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);
        connectionArt.transform.localScale = new Vector2(offset.magnitude, 0.05f);
        Destroy(connectionArt, 1.6f);
    }
}
