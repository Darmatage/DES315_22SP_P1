using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class CanGrab : MonoBehaviour
{
  [Header("Grabbed = DisableScripts?")]
  public bool disableScriptsWhenGrabbed_ = true;
  public GameObject seperateSprite_ = null;
  public bool gridlessPlacement_ = true;

  [Header("Tile Settings")]
  public bool gridPlacement_ = false;
  public Tilemap placementMap_;
  public bool lockWhenAnchored_ = false;
  public Color lockColor_;
  
  [Header("Base Debugs")]
  public bool grabbed_ = false;
  public bool change_ = false;
  public bool locked_ = false;

  [Header("Tile Debugs")]
  public Vector3Int pos_;
  public bool willFill_ = false;
  public Vector3Int gridPos_ = Vector3Int.zero;
  public string aboveThisTile_ = "";
  public int pastLayer_;


  public CanGrab()
  {
    //
  }

  public virtual void Start()
  {
    if (gridPlacement_)
    {
      gridPos_ = MousePickupScript.instance_.Vec3toVec3Int(transform.position);
      MousePickupScript.instance_.AddToGrid(this);
    }

    if(locked_)
    {
      if (seperateSprite_)
        seperateSprite_.GetComponent<SpriteRenderer>().color = lockColor_;
      else
      {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        if (sr)
          sr.color = lockColor_;
      }
    }
  }

  // Update is called once per frame
  public virtual void Update()
  {
    if(grabbed_)
      WhileHeld();

    //this part only works if disableOtherScripts_ is on
    if (disableScriptsWhenGrabbed_ && change_)
    {
      //Disables all scripts besides this if grabbed
      MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();
      foreach (MonoBehaviour script in scripts)
      {
        if (script == (MonoBehaviour)this)
        {
          continue;
        }

        if (grabbed_)
          script.enabled = false;
        else
          script.enabled = true;
      }

      change_ = false;
    }
  }

  public virtual void Grab()
  {
    grabbed_ = true;
    change_ = true;

    pastLayer_ = gameObject.layer;
    gameObject.layer = 13;

    if(gridPlacement_)
    {
      MousePickupScript.instance_.RemoveFromGrid(this);
    }
  }

  public virtual void WhileHeld()
  {
    
      
    //grid prior
    if (gridPlacement_)
    {
      //get location on grid
      gridPos_ = MousePickupScript.instance_.GetCursorIntPosition();
      Vector3Int pos = placementMap_.WorldToCell(gridPos_);

      //if on new location
      if (pos_ != pos)
      {
        //remove tags
        placementMap_.SetTileFlags(pos, TileFlags.None);
        //set previously hovered tile to white
        placementMap_.SetColor(pos_, Color.white);

        //update pos
        pos_ = pos;

        

        //check if not already taken
        if (!MousePickupScript.instance_.GridSpotTaken(gridPos_))
        {
          //Debug.Log(placementMap_.GetTile(pos).name);

          //raycast to tile, if hit, it exists
          Vector2Int vec = Vector2Int.zero;
          vec.x += gridPos_.x;
          vec.y += gridPos_.y;



          if(placementMap_.GetTile(pos) != null)
          {
            aboveThisTile_ = placementMap_.GetTile(pos).name;

            placementMap_.SetColor(pos, Color.red);

            willFill_ = true;
            MousePickupScript.instance_.CanLetGoToggle(true);
            return;
          }
        }
        willFill_ = false;
        MousePickupScript.instance_.CanLetGoToggle(false);
      }
    }

    //try free replacement
    if(gridlessPlacement_)
    {
      MousePickupScript.instance_.CanLetGoToggle(true);
      return;
    }


  }

  public virtual void Release()
  {
    if (gridPlacement_ && willFill_)
    {
      AdditionalGridRelease();

      //returns the hovered over tile color to white
      placementMap_.SetColor(pos_, Color.white);

      //snap to grid
      transform.position = new Vector3(pos_.x + 0.5f, pos_.y + 0.5f, transform.position.z);

      //gameObject.GetComponent<BoxCollider2D>().enabled = true;

      MousePickupScript.instance_.AddToGrid(this);

      if (lockWhenAnchored_)
      {
        locked_ = true;
        if(seperateSprite_)
          seperateSprite_.GetComponent<SpriteRenderer>().color = lockColor_;
        else
        {
          SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
          if(sr)
            sr.color = lockColor_;
        }
      }
    }
    gameObject.layer = pastLayer_;
    grabbed_ = false;
    change_ = true;
  }

  public virtual void AdditionalGridRelease()
  {

  }
}
