using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethMerrillPickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>().Heal(1000);
			Destroy(gameObject);
		}
	}
}
