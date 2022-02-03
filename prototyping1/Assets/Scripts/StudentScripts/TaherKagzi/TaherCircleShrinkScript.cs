using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaherCircleShrinkScript : MonoBehaviour
{
  public float shrinkSpeed = 2.0f;
  public float startScale = 10.0f;
  public float endScale = 2.0f;
  private float currentScale;
  private Vector3 tempscale;

  // Start is called before the first frame update
  void Start()
  {
    currentScale = endScale;
    tempscale.Set(endScale, endScale, 1.0f);
    transform.localScale = tempscale;
  }
  
  // Update is called once per frame
  void Update()
  {
    currentScale = transform.localScale.x;
    if (currentScale > endScale)
    {
      currentScale -= shrinkSpeed * Time.deltaTime;
    }
  
    else
    {
      currentScale = endScale;
    }
  
    tempscale.Set(currentScale, currentScale, 1.0f);
    transform.localScale = tempscale;
  }
}
