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

    private bool on_internal = false;


    void Start()
    {
        on_internal = on;
        if (on_internal)
            ArtSwitchOn();
        else
            ArtSwitchOff();

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
        Vector3 targetPosition;
        if (target is GameObject)
            targetPosition = ((GameObject)target).transform.position;
        else if (target is Component)
            targetPosition = ((Component)target).transform.position;
        else
            return;

        ParticleSystem.EmitParams particleParams = new ParticleSystem.EmitParams();

        Vector3 offset = targetPosition - transform.position;
        particleParams.position += new Vector3(0f, 0f, -0.01f);
        particleParams.velocity = offset.normalized * 10f;
        particleParams.startLifetime = offset.magnitude / 10f;
        GetComponent<ParticleSystem>().Emit(particleParams, 1);
    }
}
