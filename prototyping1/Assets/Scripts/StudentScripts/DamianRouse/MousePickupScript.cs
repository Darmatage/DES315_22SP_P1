using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePickupScript : MonoBehaviour
{
  public MousePickupScript instance_;
  public GameObject holding_;
  public GameObject mayHold_;
  public bool grid_;
  public bool canGrab_;
  public bool canLetGo_;
  public bool change_;

  public GameObject cursor_;
  public Color black_;
  public Color white_;
  public Color red_;
  public Color green_;

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
    //Vector3 mouse = Input.mousePosition;
    //Ray castPoint = Camera.main.ScreenPointToRay(mouse);
    //RaycastHit hit;
    //if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
    //{
    //  cursor_.transform.position = hit.point;
    //}

    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

    float increase = 0;//Input.mouseScrollDelta.y;

    cursor_.transform.position = new Vector3(worldPosition.x, worldPosition.y, cursor_.transform.position.z + increase);


    if (Input.GetButtonDown("Fire1"))
    {
      if(!holding_ && mayHold_)
      {
        Grab();
      } 
      else if(holding_)
      {
        Release();
      }
    }

    

    if (change_)
    {
      var renderer = cursor_.GetComponent<Renderer>();

      if (holding_ == null && canGrab_ == false)
        renderer.material.SetColor("_Color", white_);
      else if (holding_ == null && canGrab_ == true)
        renderer.material.SetColor("_Color", green_);

      change_ = false;
    }

    //RaycastHit2D cast = Physics2D.BoxCast(worldPosition, new Vector2(1,1), 0, Vector2.zero);
    RaycastHit2D cast =Physics2D.Raycast(worldPosition + Vector2.up, -Vector2.up);
    RaycastHit2D cast2 = Physics2D.Raycast(worldPosition - Vector2.up, Vector2.up);

    if (cast && cast2)
    {
      if(cast.collider.transform.position == cast2.collider.transform.position)
      {
        CanGrab cG = cast.collider.gameObject.GetComponent<CanGrab>();
        if (cG)
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
      //Debug.Log("nothing");
  }

  void Grab()
  {
    holding_ = mayHold_;
    holding_.transform.SetParent(cursor_.transform);
  }

  void Release()
  {
    holding_.transform.SetParent(null);
    holding_ = null;
    change_ = true;
  }
}
