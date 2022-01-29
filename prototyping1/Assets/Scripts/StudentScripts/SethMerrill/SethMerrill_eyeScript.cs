using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethMerrill_eyeScript : MonoBehaviour
{
	public GameObject beamPrefab;
	private GameObject beamInstance;
	public float speed = 1.0f;
	private Vector3 direction;
	static System.Random RNG;
	public float hunger;
	public float range;
	public int damage;
	private GameHandler gh;
    // Start is called before the first frame update
    void Start()
    {
		beamInstance = Instantiate(beamPrefab, transform.position, Quaternion.identity);
		beamInstance.transform.localScale = new Vector3(1.0f, range, 1.0f);
		beamInstance.GetComponent<SethMerrill_beamScript>().range = range;
		if(RNG == null) RNG = new System.Random();
		gh = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 pos = transform.position;
		Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
		
        beamInstance.transform.position = transform.position;
		
		//direction.x = (Vector3.Normalize(playerPos - pos) * hunger).x + ((float)RNG.NextDouble() - 0.5f) * (1.0f - hunger);
		//direction.y = (Vector3.Normalize(playerPos - pos) * hunger).y + ((float)RNG.NextDouble() - 0.5f) * (1.0f - hunger);	
		
		//transform.position += Vector3.Normalize(direction) * speed * Time.deltaTime;
    }
}
