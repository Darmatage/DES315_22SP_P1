using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CanGrabFillTile : CanGrab
{
  [Header("FillTile Settings")]
  public TileBase fillerTile_;
  public Tilemap mapToReplace_;
  public Tilemap mapToFill_;
  public Color anchoredColor_;
  public bool lockWhenAnchored_ = true;

  [Header("FillTile Debugs")]
  public Vector3Int pos_;
  public bool willFill_ = false;


  public CanGrabFillTile() : base()
  {
  }

  public override void WhileHeld()
  {
    Vector3Int pos = mapToReplace_.WorldToCell(MousePickupScript.instance_.GetCursorIntPosition());

    if (pos_ != pos)
    {
      mapToReplace_.SetTileFlags(pos, TileFlags.None);
      mapToReplace_.SetColor(pos, Color.red);
      mapToReplace_.SetColor(pos_, Color.white);

      pos_ = pos;
      //TileBase tb = mapToReplace_.GetTile(pos);
      //TileData td;
      //tb.GetTileData(pos, mapToReplace_, ref td);
    }

    //check if placement will fill
    if (mapToReplace_.GetTile(pos_) == null)
      willFill_ = false;
    else
      willFill_ = true;
  }

  public override void Release()
  {
    grabbed_ = false;
    change_ = true;

    if (willFill_)
    {
      if (mapToReplace_)
      {
        Vector3Int pos = mapToReplace_.WorldToCell(pos_);
        mapToReplace_.SetTile(pos, null);
      }
      if (mapToFill_ && fillerTile_)
      {
        Vector3Int pos = mapToFill_.WorldToCell(pos_);
        mapToFill_.SetTile(pos, fillerTile_);
      }

      transform.position = new Vector3(pos_.x + 0.5f, pos_.y + 0.5f, transform.position.z);

      gameObject.GetComponent<BoxCollider2D>().enabled = false;
      sprite_.GetComponent<SpriteRenderer>().color = anchoredColor_;

      if (lockWhenAnchored_)
        locked_ = true;
    }


    //TileBase tb = mapToReplace_.GetTile(pos_);





    //
  }
}
