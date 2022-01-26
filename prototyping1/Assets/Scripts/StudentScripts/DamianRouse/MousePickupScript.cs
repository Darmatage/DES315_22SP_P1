using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePickupScript : MonoBehaviour
{
  [Header("Main Debugs")]
  public static MousePickupScript instance_;
  public GameObject holding_;
  public GameObject mayHold_;
  public bool grid_;
  public bool canGrab_;
  public bool canLetGo_;
  public bool change_;

  [Header("Cursor Settings")]
  public GameObject cursor_;
  
  public Color black_;
  public Color white_;
  public Color red_;
  public Color green_;
  [Header("Cursor Debugs")]
  public Vector2 worldPosition_;

  // Start is called before the first frame update
  void Awake()
  {
    if (instance_ != null)
    {
      Debug.Log("Too many MousePickupScript");
    }
    instance_ = this;
  }

  // Update is called once per frame
  void Update()
  {
    MoveCursorToMouse();
    CheckIfOverGrabbable();
    ClickCheck();
    UpdateCursor();
  }

  void MoveCursorToMouse()
  {
    //Vector3 mouse = Input.mousePosition;
    //Ray castPoint = Camera.main.ScreenPointToRay(mouse);
    //RaycastHit hit;
    //if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
    //{
    //  cursor_.transform.position = hit.point;
    //}

    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    worldPosition_ = Camera.main.ScreenToWorldPoint(screenPosition);

    float increase = 0;//Input.mouseScrollDelta.y;

    cursor_.transform.position = new Vector3(worldPosition_.x, worldPosition_.y, cursor_.transform.position.z + increase);

  }

void CheckIfOverGrabbable()
  {
    

    //RaycastHit2D cast = Physics2D.BoxCast(worldPosition_, new Vector2(1,1), 0, Vector2.zero);
    RaycastHit2D cast = Physics2D.Raycast(worldPosition_ + Vector2.up, -Vector2.up);
    RaycastHit2D cast2 = Physics2D.Raycast(worldPosition_ - Vector2.up, Vector2.up);

    if (cast && cast2)
    {
      if (cast.collider.transform.position == cast2.collider.transform.position)
      {
        CanGrab cG = cast.collider.gameObject.GetComponent<CanGrab>();
        if (cG && cG.locked_ == false)
        {
          mayHold_ = cast.collider.gameObject;
          change_ = true;
          canGrab_ = true;
          return;
        }
      }
      //Debug.Log(cast.collider.name + "  " + cast2.collider.name); 
    }

    if (canGrab_)
      change_ = true;
    canGrab_ = false;
    mayHold_ = null;
  }

  void ClickCheck()
  {
    if (Input.GetButtonDown("Fire1"))
    {
      if (!holding_ && mayHold_)
      {
        Grab();
      }
      else if (holding_)
      {
        Release();
      }
    }
  }

  void Grab()
  {
    holding_ = mayHold_;
    holding_.transform.SetParent(cursor_.transform);

    CanGrab cG = holding_.GetComponent<CanGrab>();
    if (cG)
      cG.Grab();
  }

  void Release()
  {
    CanGrab cG = holding_.GetComponent<CanGrab>();
    if (cG)
    {
      if (cG.restrictPlacement_ == true)
        return;
      cG.Release();
    }

    holding_.transform.SetParent(null);
    holding_ = null;
    change_ = true;
  }

  void UpdateCursor()
  {
    if (change_)
    {
      var renderer = cursor_.GetComponent<Renderer>();

      if (holding_ == null && canGrab_ == false)
        renderer.material.SetColor("_Color", white_);
      else if (holding_ == null && canGrab_ == true)
        renderer.material.SetColor("_Color", green_);

      change_ = false;
    }
  }

  public Vector3Int GetCursorIntPosition()
  {
    Vector3 vec = cursor_.transform.position;
    return new Vector3Int(Mathf.RoundToInt(vec.x - 0.8f), Mathf.RoundToInt(vec.y - 0.3f), (int)vec.z);
  }
}
