using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KennyMecham_ReflectorBase : MonoBehaviour
{
  public List<KennyMecham_AttackDetectorBase> detectors_;

  // Start is called before the first frame update
  public void Init()
  {
    foreach (var detector in detectors_)
    {
      detector.AddOnDetectionEvent(OnDetectorTrigger);
      detector.Deactivate();
    }
  }

  public void ActivateDetectors()
  {
    foreach(var detector in detectors_)
    {
      detector.Activate(gameObject);
    }
  }

  public void DeactivateDetectors()
  {
    foreach (var detector in detectors_)
    {
      detector.Deactivate();
    }
  }

  public abstract void OnDetectorTrigger(GameObject obj);
}
