using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_AttackDetectorFromType : KennyMecham_AttackDetectorBase
{
  public override bool ShouldDetectorTrigger(GameObject other)
  {
    return !(other.GetComponent<KennyMecham_ReflectableAttackBase>() is null);
  }
}
