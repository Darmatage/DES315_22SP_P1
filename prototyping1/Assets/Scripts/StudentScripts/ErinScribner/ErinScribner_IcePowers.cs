using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//the purpose of this script is to have a walkable block be placed on a block of lava that the
//player character is looking at
public class ErinScribner_IcePowers : MonoBehaviour
{
    private GameHandler gameHandlerObj;
    private Transform playerTrans;
    public GameObject iceBlock;
    public int maxNum = 4;
    private int limit = 0;

    public Tilemap destructableTilemap;
    private List<Vector3> tileWorldLocations;
    public float rangeDestroy = 2f;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("GameHandler") != null)
        {
            gameHandlerObj = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        }
        playerTrans = GameObject.FindWithTag("Player").GetComponent<Transform>();
        limit = maxNum;

        Init();
    }


    void Init()
    {
        tileWorldLocations = new List<Vector3>();

        foreach (var pos in destructableTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = destructableTilemap.CellToWorld(localPlace) + new Vector3(0.5f, 0.5f, 0f);

            if (destructableTilemap.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && limit > 0)
        {
           // GameObject iceblock = GameObject.Find("IceBlock");
            Vector3 playerFeet = new Vector3(playerTrans.position.x, playerTrans.position.y - .8f, playerTrans.position.z);
            GameObject iceBlockNew = Instantiate(iceBlock, playerFeet, Quaternion.identity);

            Vector3 playerFeet2 = new Vector3(playerTrans.position.x - 1.0f, playerTrans.position.y, playerTrans.position.z);
            GameObject iceBlockNew2 = Instantiate(iceBlock, playerFeet2, Quaternion.identity);

            Vector3 playerFeet3 = new Vector3(playerTrans.position.x + 1.0f, playerTrans.position.y, playerTrans.position.z);
            GameObject iceBlockNew3 = Instantiate(iceBlock, playerFeet3, Quaternion.identity);

            Vector3 playerFeet4 = new Vector3(playerTrans.position.x, playerTrans.position.y + 1, playerTrans.position.z);
            GameObject iceBlockNew4 = Instantiate(iceBlock, playerFeet4, Quaternion.identity);

            Vector3 playerFeet5 = new Vector3(playerTrans.position.x, playerTrans.position.y, playerTrans.position.z);
            GameObject iceBlockNew5 = Instantiate(iceBlock, playerFeet5, Quaternion.identity);

            limit--;

            // GameObject player = GameObject.Find("Player");

            //iceblock.transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y - .8f, playerTrans.position.z);

            destroyTileArea();
        }
    }

    void destroyTileArea()
    {
        foreach (Vector3 tile in tileWorldLocations)
        {
            if (Vector2.Distance(tile, playerTrans.position) <= rangeDestroy)
            {
                //Debug.Log("in range");
                Vector3Int localPlace = destructableTilemap.WorldToCell(tile);
                if (destructableTilemap.HasTile(localPlace))
                {
                    //StartCoroutine(BoomVFX(tile));
                    destructableTilemap.SetTile(destructableTilemap.WorldToCell(tile), null);
                }
                //tileWorldLocations.Remove(tile);
            }
        }
    }
}
