using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrankieCamarilloFireBlast : MonoBehaviour
{
    private GameObject player_;
    public GameObject FireProjectile;
    private Camera cam_;

    public int damage_ = 1;
    public float range_ = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        cam_ = Camera.main;
        player_ = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(1))
        {
            // this is where I am going to spawn the prefab

            /*
            Vector3 cameraPoint = Input.mousePosition;
            cameraPoint.z = Vector3.Distance(cam_.transform.position, player_.transform.position);
            Vector3 mousePos = cam_.ScreenToWorldPoint(cameraPoint);
           
            
            Vector3 playerToMouse = mousePos - player_.transform.position ;
            Vector3 spawnPos = new Vector3(player_.transform.position.x, player_.transform.position.y, 0);
            GameObject newSpawn = Instantiate(FireProjectile, spawnPos, Quaternion.Euler(playerToMouse));
            */


            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 direction = mouseWorld;

            float AngleRad = Mathf.Atan2(direction.y - player_.transform.position.y, direction.x - player_.transform.position.x);

            float AngleDeg = (180 / Mathf.PI) *AngleRad;

            Vector3 spawnPos = new Vector3(player_.transform.position.x, player_.transform.position.y, 0);
            Instantiate(FireProjectile, spawnPos, Quaternion.Euler(0, 0, AngleDeg));

        }
    }
}
