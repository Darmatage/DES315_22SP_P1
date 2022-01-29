using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_DefaultReflector : KennyMecham_ReflectorBase
{
  private void Start()
  {
     Init();
  }

  public override void OnDetectorTrigger(GameObject obj)
  {
    obj.GetComponent<KennyMecham_ReflectableAttackBase>().Reflect(gameObject);
  }
}
