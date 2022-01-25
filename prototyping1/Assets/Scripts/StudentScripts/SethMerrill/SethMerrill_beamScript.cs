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
		Vector3 pos = transform.position;
		Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
		
		Vector3 v = Vector3.Normalize(playerPos - pos);
		float f = Vector2.Angle(v, new Vector2(0,1));
		
		Vector3 q = transform.eulerAngles;
		q.z = f;
		transform.eulerAngles = q;
		
		//Debug.Log(gh.gameObject.name);
    }
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			gh.TakeDamage(damage);
		}
	}
}
