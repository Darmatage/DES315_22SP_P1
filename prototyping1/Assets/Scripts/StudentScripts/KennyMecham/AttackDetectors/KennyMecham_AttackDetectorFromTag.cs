using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_AttackDetectorFromTag : KennyMecham_AttackDetectorBase
{
  public GameObject parent;
  public List<string> tags = new List<string>();
  private List<string> coTags;

  // Start is called before the first frame update
  void Start()
  {
    if(parent is null)
    {
      coTags = COTags.GetAllGameObjectTags(gameObject);
    }
    else
    {
      coTags = COTags.GetAllGameObjectTags(parent);
    }
  }

  public override bool ShouldDetectorTrigger(GameObject other)
  {
    return (!(COTags.CompareAnyTags(other, tags) || COTags.CompareAnyTags(other, coTags)));
  }
}
