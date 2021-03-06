using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethMerrill_beamScript : MonoBehaviour
{
	private GameHandler gh;
	public int damage;
	Vector3 pos;
	Vector3 playerPos;
	public float range;
	public GameObject parentObj;
    // Start is called before the first frame update
    void Start()
    {
		gh = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
		
    }

    // Update is called once per frame
    void Update()
    {
		pos = transform.position;
		Vector2 pp = GameObject.FindWithTag("Player").transform.position;
		playerPos = pp + GameObject.FindWithTag("Player").GetComponent<Collider2D>().offset;
		Debug.DrawRay(pos, playerPos-pos, Color.red);
		LookAround();
		//Debug.Log(gh.gameObject.name);
    }
	
	void See()
	{
		float dist=1;
		Vector3 start = transform.position+dist*Vector3.Normalize(playerPos - pos);
		Vector3 direction = Vector3.Normalize(playerPos - pos);
		float rayDist = range-dist;
		
		RaycastHit2D rc = Physics2D.Raycast(start, direction, rayDist);
		Debug.DrawRay(start, direction*(range-dist));
		if(rc.collider)
		{
			//Debug.Log(rc.collider.gameObject.name);
			if(rc.collider.gameObject.tag == "Player")
			{
				gh.TakeDamage(damage);
				parentObj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
				AudioSource[] audio = parentObj.GetComponents<AudioSource>();
				audio[1].Play();
			}
			//transform.localScale = new Vector3(1.0f, Vector2.Distance(rc.collider.transform.position, pos), 1.0f);
		}
	}
	
	void LookAround()
	{
		Vector3 q = transform.eulerAngles;
		Vector3 v = Vector3.Normalize(playerPos - pos);
		float f = Vector2.Angle(v, new Vector2(0,1));
		float f2 = Vector2.Angle(v, new Vector2(0,-1));
		
		q.z+=.5f;
		transform.eulerAngles = q;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		See();
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		parentObj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
	}
	
}
