using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarryChen_FogOfWar_HealthPickup : MonoBehaviour
{
    private GameHandler gameHandlerObj;
	public int HealthRestore = 10;

    void Start()
    {
        GameObject gameHandlerLocation = GameObject.FindWithTag ("GameHandler");
		if (gameHandlerLocation != null) {
			gameHandlerObj = gameHandlerLocation.GetComponent<GameHandler> ();
		}
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            gameHandlerObj.Heal(HealthRestore);
            Destroy (gameObject);
        }
    }
}
