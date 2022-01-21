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
	public UnityEvent OnSwitchEvent;

	public bool toggle = false;

	public List<string> tags = new List<string>{ "Player" };

	private bool on = false;

	void Start()
	{
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
		// don't un-switch if not desired.
		if (toggle || on)
		{
			foreach (string t in tags)
			{
				if (other.gameObject.tag == t)
				{
					if (on)
						ArtSwitchOn();
					else
						ArtSwitchOff();

					OnSwitchEvent.Invoke();

					break;
				}
			}
		}
	}
}
