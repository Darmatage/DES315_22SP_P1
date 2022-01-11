using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethMerrill_eyeScript : MonoBehaviour
{
	public GameObject beamPrefab;
	private GameObject beamInstance;
	public float speed = 1.0f;
	public Vector3 direction;
	static System.Random RNG;
	public float hunger;
    // Start is called before the first frame update
    void Start()
    {
		beamInstance = Instantiate(beamPrefab, transform.position, Quaternion.identity);
		direction = new Vector3(1, 0, 0);
		if(RNG == null) RNG = new System.Random();
		hunger = (float)RNG.NextDouble();
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 pos = transform.position;
		Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
		
        beamInstance.transform.position = transform.position;
		
		direction.x = (Vector3.Normalize(playerPos - pos) * hunger).x + ((float)RNG.NextDouble() - 0.5f) * (1.0f - hunger);
		direction.y = (Vector3.Normalize(playerPos - pos) * hunger).y + ((float)RNG.NextDouble() - 0.5f) * (1.0f - hunger);
		
		transform.position += Vector3.Normalize(direction) * speed * Time.deltaTime;
    }
}
