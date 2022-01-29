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

	private bool on_internal = false;


	void Start()
	{
		on_internal = on;
		if (on_internal)
			ArtSwitchOn();
		else
			ArtSwitchOff();
	}

	private void ArtSwitchOn()
	{
		SwitchOffArt.SetActive(false);
		SwitchOnArt.SetActive(true);
	}

	private void ArtSwitchOff()
	{
		SwitchOffArt.SetActive(true);
		SwitchOnArt.SetActive(false);
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
}
