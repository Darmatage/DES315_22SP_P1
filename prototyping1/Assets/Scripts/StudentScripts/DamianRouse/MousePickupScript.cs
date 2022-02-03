using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePickupScript : MonoBehaviour
{
  public static MousePickupScript instance_;

  [Header("Layer Settings")]
  public LayerMask layersWithPickupObjects_;
  [Header("Cursor Settings")]
  public GameObject cursor_;


  public Color black_;
  public Color white_;
  public Color red_;
  public Color green_;
  [Header("Cursor Debugs")]
  public Vector2 worldPosition_;

  [Header("Main Debugs")]

  public GameObject holding_;
  public GameObject mayHold_;
  public bool grid_;
  public bool canGrab_;
  public bool canLetGo_ = true;
  public bool change_;
  public List<CanGrab> onGrid_ = new List<CanGrab>();

  // Start is called before the first frame update
  void Awake()
  {
    //singleton
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
    //get the position of the mouse in world space
    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    worldPosition_ = Camera.main.ScreenToWorldPoint(screenPosition);

    //move the cursor to position
    cursor_.transform.position = new Vector3(worldPosition_.x, worldPosition_.y, cursor_.transform.position.z);
  }

  void CheckIfOverGrabbable()
  {
    //raw cast down from mouse
    RaycastHit2D hit = Physics2D.Raycast(worldPosition_, Vector2.up, 0.01f, layersWithPickupObjects_);
    if (hit.collider != null)
    {
      //pickup not locked grabbables
      CanGrab cG = hit.collider.gameObject.GetComponent<CanGrab>();
      if (cG && cG.locked_ == false)
      {
        mayHold_ = hit.collider.gameObject;
        change_ = true;
        canGrab_ = true;
        return;
      }

    }

    //Nothing to grab
    if (canGrab_)
      change_ = true;
    canGrab_ = false;
    mayHold_ = null;
  }

  void ClickCheck()
  {
    //click
    if (Input.GetButtonDown("Fire1"))
    {
      //grab if not holding
      if (!holding_ && mayHold_)
      {
        Grab();
      }
      else if (holding_ && canLetGo_)
      {
        Release();
      }
    }
  }

  void Grab()
  {
    //hold what was holdable
    holding_ = mayHold_;
    //set parent
    holding_.transform.SetParent(cursor_.transform);

    //Tell the grabbed object to do what it should do
    CanGrab cG = holding_.GetComponent<CanGrab>();
    if (cG)
      cG.Grab();
  }

  void Release()
  {
    //Tell the grabbed object to do release scripts if it can be released
    CanGrab cG = holding_.GetComponent<CanGrab>();
    if (cG)
    {
      cG.Release();
    }

    //empty grabber
    holding_.transform.SetParent(null);
    holding_ = null;
    change_ = true;
  }

  void UpdateCursor()
  {
    //update the color of the cursor based of if it's can/is holding
    if (change_)
    {
      var renderer = cursor_.GetComponent<Renderer>();

      if (holding_ == null && canGrab_ == false)
        renderer.material.SetColor("_Color", white_);
      else if (holding_ == null && canGrab_ == true)
        renderer.material.SetColor("_Color", green_);
      else if (holding_ == true && canLetGo_ == false)
        renderer.material.SetColor("_Color", red_);
      else if (holding_ == true && canLetGo_ == true)
        renderer.material.SetColor("_Color", green_);

      change_ = false;
    }
  }

  public Vector3Int GetCursorIntPosition()
  {
    Vector3 vec = cursor_.transform.position;
    //offset for better placement
    return Vec3toVec3Int(vec);
  }

  public Vector3Int Vec3toVec3Int(Vector3 vec)
  {
    //have the vec be in accordance to cursor
    return new Vector3Int(Mathf.RoundToInt(vec.x - 0.8f), Mathf.RoundToInt(vec.y - 0.3f), (int)cursor_.transform.position.z);
  }

  public void CanLetGoToggle(bool status)
  {
    if (status == canLetGo_)
      return;
    canLetGo_ = status;
    change_ = true;
  }

  public void AddToGrid(CanGrab cG)
  {
    onGrid_.Add(cG);
  }

  public void RemoveFromGrid(CanGrab cG)
  {
    onGrid_.Remove(cG);
  }

  public bool GridSpotTaken(Vector3Int pos)
  {
    foreach(CanGrab c in onGrid_)
    {
      if(c)
      {
        if (c.gridPos_ == pos)
          return true;
      }
    }
    return false;
  }
}
