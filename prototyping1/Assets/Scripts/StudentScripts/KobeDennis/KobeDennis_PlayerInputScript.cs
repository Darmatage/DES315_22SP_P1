using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class KobeDennis_PlayerInputScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 fireDirection;
    [SerializeField]
    private Vector3 lastFireDirection;
    public GameObject Lavaball_prefab;
    private Transform playerTransform;
    private Tilemap lavaTilemap;

    [Header("Lava Projectile Settings")]
    public KeyCode fireKey = KeyCode.Space;
    public float fireRate = 1.0f;
    private float nextFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        fireDirection = Vector3.zero;
        playerTransform = GameObject.FindWithTag("Player").transform;
        SpawnLavaTIleMap();

    }
    private void Update()
    {
        if (Input.GetKeyDown(fireKey) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            var offset = Vector3.zero;

            //Reposition the projectile if the player is shooting down
            if (lastFireDirection.y <= -1f)
            {
                offset = new Vector3(0, -1f, 0f);

            }
            var lavaBall = Instantiate(Lavaball_prefab, playerTransform.position + offset + lastFireDirection, Quaternion.identity) as GameObject;

            lavaBall.GetComponent<KobeDennis_LavaBallScript>().SetDirection(lastFireDirection);

        }
    }
    private void SpawnLavaTIleMap()
    {

       
            GameObject obj = new GameObject();

            obj.name = "TilemapLavaKobe";

            obj.AddComponent<Tilemap>();
            obj.AddComponent<TilemapRenderer>();
            obj.AddComponent<Rigidbody2D>();
            obj.AddComponent<TilemapCollider2D>();
            obj.AddComponent<CompositeCollider2D>();
            obj.AddComponent<Lava>();

            obj.transform.parent = GameObject.Find("Grid").transform;
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            obj.GetComponent<TilemapRenderer>().sortingOrder = 5;
            obj.GetComponent<TilemapCollider2D>().usedByComposite = true;
            obj.GetComponent<CompositeCollider2D>().isTrigger = true;
            obj.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Polygons;

            obj.GetComponent<CompositeCollider2D>().generationType = CompositeCollider2D.GenerationType.Synchronous;

            lavaTilemap = obj.GetComponent<Tilemap>();
        }
        // Update is called once per frame
        void FixedUpdate()
    {
        fireDirection = Vector3.zero;
        fireDirection.x = Input.GetAxisRaw("Horizontal");
        fireDirection.y = Input.GetAxisRaw("Vertical");
        if(fireDirection != Vector3.zero)
        lastFireDirection = fireDirection;


        if(fireDirection.x > 0 )
        {
            Debug.Log("Right");

        }
        else if (fireDirection.x < 0)
        {
            Debug.Log("Left");

        }
        
    }
}
