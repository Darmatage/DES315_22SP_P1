using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamianRouse_AI_Hijack : MonoBehaviour
{
  [Header("Settings")]
  public GameObject player_;
  public float distance_ = 5;
  public bool all_;
  public List<MonoBehaviour> specifics_ = new List<MonoBehaviour>();

  [Header("Debugs")]
  public bool enabled_ = false;
  public bool change_ = false;

  void Start()
  {
    if(player_ == null)
    {
      player_ = GameObject.Find("Player");
      if (player_ == null)
        Debug.Log("Couldn't find player...");
    }
    //Disables scripts at the start
    ScriptUpdate();
  }

  // Update is called once per frame
  void Update()
  {
    //disable/renable based on enable_
    if (change_)
    {
      ScriptUpdate();
      change_ = false;
    }

    //if distance is 0, distance wont affect enable
    if (distance_ == 0)
      return;

    //Get distance between ai and player
    Vector2 pVec = new Vector2(player_.transform.position.x, player_.transform.position.y);
    Vector2 tVec = new Vector2(transform.position.x, transform.position.y);
    float dist = Vector2.Distance(pVec, tVec);

    //if exceed distance turn off
    if (dist > distance_)
    {
      if(enabled_)
      {
        change_ = true;
        enabled_ = false;
      }
    }
    else
    {
      if (!enabled_)
      {
        change_ = true;
        enabled_ = true;
      }
    }

  }

  public void ScriptUpdate()
  {
    //all scripts or specific scripts
    if (all_)
      ScriptUpdateAll(enabled_);
    else
      ScriptUpdateSpecific(enabled_);
  }

  public void ScriptUpdateAll(bool enable)
  {
    //retrieve all scripts
    MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();
    foreach (MonoBehaviour script in scripts)
    {
      //do not disable itself
      if (script == (MonoBehaviour)this)
      {
        continue;
      }

      script.enabled = enable;
    }
  }
  public void ScriptUpdateSpecific(bool enable)
  {
    //use specific scripts
    foreach (MonoBehaviour script in specifics_)
    {
      script.enabled = enable;
    }
  }

}
