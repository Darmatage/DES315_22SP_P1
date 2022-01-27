using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class KobeDennis_LavaBallScript : MonoBehaviour
{
    public float speed;
    public float LifeTime = 0f;
    public float lavaTileSpawnRate = 0.5f;
    private float delayTime = 0.15f;
    private bool isDelay = true;
    private float lastSpawnedRate = 0f;
    private Rigidbody2D rb;
    private Vector3 direction;
    public GameObject lavaTile_obj;
    public GameObject explosion;


    public TileBase lavaTile;
    private Tilemap lavaTilemap;



    public void SetDirection(Vector3 dir)
    {
        direction = dir;

        //Change the direction of the sprite
        if(direction.x <= -1f)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (direction.x >= 1f)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (direction.y <= -1f)
        {
            gameObject.transform.GetChild(0).transform.Rotate(0.0f, 0.0f,-90.0f, Space.Self);

        }
        else if (direction.y >= 1f)
        {
            gameObject.transform.GetChild(0).transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);

        }

    }
    private void Update()
    {
        if(isDelay)
        {
            delayTime -= Time.deltaTime;

            if(delayTime <= 0f)
            {
                isDelay = false;
            }
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = (Vector2)direction * speed;

        if(!isDelay)
        {
            if (lavaTilemap)
            {
                if (Time.time > lavaTileSpawnRate + lastSpawnedRate)
                {
                    //Vector3Int n = Vector3Int.FloorToInt(transform.position);
                    Instantiate(lavaTile_obj, transform.position, Quaternion.identity);
                    lastSpawnedRate = Time.time;
                }
            }
        }
       

    }
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, LifeTime);

        GameObject o = GameObject.Find("TilemapLavaKobe");
        if (o)
            lavaTilemap = o.GetComponent<Tilemap>();



    }
    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 basePos = Camera.main.transform.position;

        float localShakeTime = 0.0f;

        while(localShakeTime < duration)
        {
            Vector3 shakePos = new Vector3(magnitude * Random.Range(-1f,1f), magnitude * Random.Range(-1f, 1f), basePos.z);
            Camera.main.transform.localPosition = shakePos;
            localShakeTime += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.localPosition = basePos;

    }
    private void CustomDestroyObject(Transform lavaTilePos = null)
    {
        if (lavaTilePos == null)
            lavaTilePos = gameObject.transform;

     //   StartCoroutine(CameraShake(0.15f,0.4f));

        Instantiate(lavaTile_obj, lavaTilePos.position, Quaternion.identity);

        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);


    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //If it collide with an enenmy or another lava ball destory itself
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.CompareTag("bullet"))
        {
            CustomDestroyObject(collision.gameObject.transform);
        }
        //For now destory this object if it collide with anything
        if (!collision.gameObject.CompareTag("Player"))
        {
            CustomDestroyObject();

          //  Destroy(gameObject);

        }


    }
}
