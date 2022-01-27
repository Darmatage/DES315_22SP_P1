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
    // Start is called before the first frame update
    void Start()
    {
		gh = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
		
    }

    // Update is called once per frame
    void Update()
    {
		pos = transform.position;
		playerPos = GameObject.FindWithTag("Player").transform.position;
		Debug.DrawRay(pos, playerPos-pos, Color.red);
		if(Vector3.Distance(pos, playerPos) <= range)
		{
			LookAt();
		}
		else
		{
			LookAround();
		}
		See();
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
			if(rc.collider.gameObject.tag == "Player")
			{
				gh.TakeDamage(damage);
			}
			else
			{
				Debug.Log(rc.collider.gameObject.name);
			}
		}
	}
	
	void LookAt()
	{
		Vector3 v = Vector3.Normalize(playerPos - pos);
		float f = Vector2.Angle(v, new Vector2(0,1));
		float f2 = Vector2.Angle(v, new Vector2(0,-1));
		
		Vector3 q = transform.eulerAngles;
		if(playerPos.x < pos.x)
			q.z = f;
		else
			q.z = -f;
		transform.eulerAngles = q;
	}
	
	void LookAround()
	{
		Vector3 q = transform.eulerAngles;
		Vector3 v = Vector3.Normalize(playerPos - pos);
		float f = Vector2.Angle(v, new Vector2(0,1));
		float f2 = Vector2.Angle(v, new Vector2(0,-1));
		
		if(Vector3.Distance(pos, playerPos) > range * 2)
		{
			q.z+=.5f;
		}
		else
		{
			if(playerPos.x < pos.x)
				q.z = f;
			else
				q.z = -f;
			transform.eulerAngles = q;
		}
		transform.eulerAngles = q;
	}
}
