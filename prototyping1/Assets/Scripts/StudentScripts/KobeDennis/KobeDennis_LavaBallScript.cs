using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class KobeDennis_LavaBallScript : MonoBehaviour
{
    public float speed;
    public float LifeTime = 0f;
    private Rigidbody2D rb;
    private Vector3 direction;

    public TileBase lavaTile;
    private Tilemap lavaTilemap;


    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
    private void FixedUpdate()
    {
        rb.velocity = (Vector2)direction * speed;

        if(lavaTilemap)
            lavaTilemap.SetTile(Vector3Int.FloorToInt(transform.position), lavaTile);

    }
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, LifeTime);

        lavaTilemap = GameObject.Find("TilemapLava").GetComponent<Tilemap>();
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //StartCoroutine(collision.collider.gameObject.GetComponent<MonsterMoveHit>().GetHit);

            Destroy(gameObject);
        }
        
    }
}
