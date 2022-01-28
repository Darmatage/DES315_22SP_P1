using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KennyMecham_AttackDetectorBase : MonoBehaviour
{
  public delegate void OnDetection(GameObject objCollidedWith);
  private OnDetection onDetection;
  private GameObject m_activator;

  public void AddOnDetectionEvent(OnDetection function)
  {
    onDetection += function;
  }

  public void Activate(GameObject activator)
  {
    m_activator = activator;
    gameObject.SetActive(true);
  }

  public void Deactivate()
  {
    m_activator = null;
    gameObject.SetActive(false);
  }
  public abstract bool ShouldDetectorTrigger(GameObject other);

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if(ShouldDetectorTrigger(collision.gameObject) && !(onDetection is null))
    {
      onDetection.Invoke(collision.gameObject);
    }
  }
}
