using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CanGrabFillTile : CanGrab
{
  [Header("FillTile Settings")]
  public TileBase fillerTile_;
  public Tilemap mapToFill_;

  public CanGrabFillTile() : base()
  {

  }

  public override void AdditionalGridRelease()
  {
    if (placementMap_ && willFill_)
    {
      Vector3Int pos = placementMap_.WorldToCell(MousePickupScript.instance_.GetCursorIntPosition());
      placementMap_.SetTile(pos, null);
      gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    if (mapToFill_ && fillerTile_)
    {
      Vector3Int pos = mapToFill_.WorldToCell(MousePickupScript.instance_.GetCursorIntPosition());
      mapToFill_.SetTile(pos, fillerTile_);
    }
  }
}
