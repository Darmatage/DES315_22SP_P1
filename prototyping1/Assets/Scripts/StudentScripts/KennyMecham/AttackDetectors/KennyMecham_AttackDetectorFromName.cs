using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_AttackDetectorFromName : KennyMecham_AttackDetectorBase
{
  public List<string> objectNames = new List<string>();

  public override bool ShouldDetectorTrigger(GameObject other)
  {
    string objName = other.name;

    foreach (string name in objectNames)
    {
      if (name.Contains(objName))
      {
        return true;
      }
    }

    return false;
  }
}
