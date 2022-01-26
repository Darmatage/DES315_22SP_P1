using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CanGrab : MonoBehaviour
{
  [Header("Grabbed = DisableScripts?")]
  public bool disableOtherScripts_ = true;
  public GameObject sprite_;

  [Header("Base Debugs")]
  public bool grabbed_ = false;
  public bool change_ = false;
  public bool locked_ = false;
  public bool restrictPlacement_ = false;

  public CanGrab()
  {
    //
  }

  public virtual void Start()
  {

  }

  // Update is called once per frame
  public virtual void Update()
  {
    if(grabbed_)
      WhileHeld();

    //this part only works if disableOtherScripts_ is on
    if (disableOtherScripts_ && change_)
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
  }

  public virtual void WhileHeld()
  {

  }

  public virtual void Release()
  {
    grabbed_ = false;
    change_ = true;
  }
}
