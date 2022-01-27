using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DamianRouse_Grid : MonoBehaviour
{

  public GameObject grid_;
  public List<Tilemap> tilemaps_ = new List<Tilemap>();
  public int xSize_ = 0;
  public int ySize_ = 0;

  public DamianRouse_Tile[][] tiles_;
  public bool[][] passible_;

  public DamianRouse_Grid instance_ = null;

  void Start()
  {
    if (instance_ != null)
    {
      Debug.Log("Too many DamianRouse_Grids in scene!");
    }
    instance_ = this;


    foreach (Transform child in grid_.transform)
    {
      Tilemap t = child.gameObject.GetComponent<Tilemap>();

      if (t)
      {
        tilemaps_.Add(t);
        BoundsInt bounds = t.cellBounds;

        if (bounds.size.x > xSize_)
          xSize_ = bounds.size.x;
        if (bounds.size.y > ySize_)
          ySize_ = bounds.size.y;
      }
    }

    tiles_ = new DamianRouse_Tile[xSize_][];
    passible_ = new bool[xSize_][];

    for (int i = 0; i != xSize_; i++)
    {
      tiles_[i] = new DamianRouse_Tile[ySize_];
      passible_[i] = new bool[ySize_];
    }

    for (int i = 0; i != xSize_; i++)
    {
      for (int j = 0; j != ySize_; j++)
      {
        tiles_[i][j] = new DamianRouse_Tile(i, j);
        passible_[i][j] = false;
      }
    }

    int lowestX = 100;
    int lowestY = 100;

    foreach (Tilemap t in tilemaps_)
    {
      BoundsInt bounds = t.cellBounds;
      Vector3Int localPlace = (new Vector3Int(t.cellBounds.xMin, t.cellBounds.yMin, (int)t.transform.position.y));
      Vector3 place = t.CellToWorld(localPlace);

      if (place.x < lowestX)
        lowestX = (int)place.x;
      if (place.y < lowestY)
        lowestY = (int)place.y;
    }

    //Debug.Log(lowestX + " " + lowestY);

    foreach (Tilemap t in tilemaps_)
    {
      BoundsInt bounds = t.cellBounds;
      TileBase[] allTiles = t.GetTilesBlock(bounds);

      Vector3Int localPlace = (new Vector3Int(t.cellBounds.xMin, t.cellBounds.yMin, (int)t.transform.position.y));
      Vector3 place = t.CellToWorld(localPlace);

      Vector2Int offset = new Vector2Int((int)Mathf.Abs(lowestX - place.x), (int)Mathf.Abs(lowestY - place.y));

      //Debug.Log(offset);

      for (int x = 0; x < bounds.size.x; x++)
      {
        for (int y = 0; y < bounds.size.y; y++)
        {
          TileBase tile = allTiles[x + y * bounds.size.x];
          if (tile != null)
          {
            tiles_[x + offset.x][y + offset.y].SetTile(t.name);
            //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
          }
          else
          {
            //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
          }
        }
      }
    }

for (int x = 0; x<xSize_; x++)
  {
    for (int y = 0; y<ySize_; y++)
    {
      //Debug.Log(tiles_[x][y].list_.Count);
    }
  }
  }

  
}

[Serializable]
public class DamianRouse_Tile
{
  int x_;
  int y_;
  public List<string> list_ = new List<string>();

  public DamianRouse_Tile(int x, int y)
  {
    x_ = x;
    y_ = y;
  }

  public void SetTile(string layer)
  {
    list_.Add(layer);
  }
}
