using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethMerrill_beamScript : MonoBehaviour
{
	private GameHandler gh;
	public int damage;
    // Start is called before the first frame update
    void Start()
    {
		gh = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 q = transform.eulerAngles;
		q.z += .1f;
		transform.eulerAngles = q;
    }
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			gh.TakeDamage(damage);
		}
	}
}
