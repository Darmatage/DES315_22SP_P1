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
        IsThereLavaTileMap();

        

    }
    private void IsThereLavaTileMap()
    {
        GameObject o = GameObject.Find("TilemapLava");
        if (o)
            lavaTilemap = o.GetComponent<Tilemap>();

        //There wasn't a lava tilemap in scene
        if (!o)
        {
            GameObject obj = new GameObject();

            obj.name = "TilemapLava";

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
