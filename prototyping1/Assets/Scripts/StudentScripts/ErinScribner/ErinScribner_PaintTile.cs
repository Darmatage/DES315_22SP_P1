using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ErinScribner_PaintTile : MonoBehaviour
{
    public Tilemap paintTilemap; //a (potentially) empty map to be painted
    public Tilemap rangeTilemap; // the entire space that can be painted
    public Tile newTile;
    private List<Vector3> tileWorldLocations;
    public float rangePaint = 1.5f;
    public bool canPaint = true;

    

    public int heal = 1;
    public float damageTime = 0.5f;
    private bool isHealing = false;
    private float damageTimer = 0f;
    private GameHandler gameHandlerObj;


    private Transform playerTrans;
    public int numPower = 2;
    private float tempcool = 0f;
    private bool cooldownDone = true;
    //public GameObject paintFX;
    public float rechargeSpeed = 0.1f;
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("GameHandler") != null)
        {
            gameHandlerObj = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        }

        playerTrans = GameObject.FindWithTag("Player").GetComponent<Transform>();

       

        TileMapInit();
    }

    void Update()
    {
        if (isHealing == true)
        {
            damageTimer += 0.1f;
            if (damageTimer >= damageTime)
            {
                gameHandlerObj.Heal(heal);
                damageTimer = 0f;
            }
        }

        if (cooldownDone == false)
        {
            tempcool -= rechargeSpeed;
        }

        if (tempcool >= numPower)
        {
            cooldownDone = false;
        }

        if(tempcool <= 0f)
        {
            cooldownDone = true;
        }

        if ((Input.GetKeyDown(KeyCode.I)) && (canPaint == true) && cooldownDone == true)
        {
            transform.position = playerTrans.position;
            destroyTileArea();
            CreateTileArea();
        }
    }

    void TileMapInit()
    {
        tileWorldLocations = new List<Vector3>();

        foreach (var pos in rangeTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = rangeTilemap.CellToWorld(localPlace) + new Vector3(0.5f, 0.5f, 0f);

            if (rangeTilemap.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);
            }
        }
    }

    void CreateTileArea()
    {
        foreach (Vector3 tile in tileWorldLocations)
        {
            if (Vector2.Distance(tile, transform.position) <= rangePaint)
            {
                //Debug.Log("in range");
                //StartCoroutine(PaintVFX(tile));
                paintTilemap.SetTile(paintTilemap.WorldToCell(tile), newTile);
            }
        }

        tempcool++;
    }

    void destroyTileArea()
    {
        foreach (Vector3 tile in tileWorldLocations)
        {
            if (Vector2.Distance(tile, transform.position) <= rangePaint)
            {
                //Debug.Log("in range");
                Vector3Int localPlace = rangeTilemap.WorldToCell(tile);
                if (rangeTilemap.HasTile(localPlace))
                {
                    //StartCoroutine(BoomVFX(tile));
                    rangeTilemap.SetTile(rangeTilemap.WorldToCell(tile), null);
                }
                //tileWorldLocations.Remove(tile);
            }
        }
    }

    //IEnumerator PaintVFX(Vector3 tilePos){
    //GameObject tempVFX = Instantiate(paintFX, tilePos, Quaternion.identity);
    //yield return new WaitForSeconds(.5f);
    //Destroy(tempVFX);
    //}

    //NOTE: To help see the attack sphere in editor:
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, rangePaint);
    }



    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            isHealing = true;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
          if (other.gameObject.tag == "Player")
          {
              isHealing = false;

          }
    }
}
