using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaherRechargeScript : MonoBehaviour
{
  private TaherCircleShrinkScript script;
  public float shrinkSpeed = 2.0f;
  public float startScale = 10.0f;
  public float endScale = 2.0f;
  private float currentScale;
  private Vector3 tempscale;

  private GameObject circleObject;

  private AudioSource audioSource;
  private float audioTimer = 0.0f;

  // Start is called before the first frame update
  void Start()
  {
    circleObject = GameObject.Find("TaherCircle");
    script = circleObject.GetComponent<TaherCircleShrinkScript>();

    shrinkSpeed = script.shrinkSpeed * 5;
    startScale = script.startScale;
    endScale = script.endScale;
    currentScale = endScale;

    audioSource = GetComponent<AudioSource>();
  }

  private void Update()
  {
    if(audioTimer >= 0.0f)
    audioTimer -= Time.deltaTime;
  }

  private void OnCollisionStay2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      currentScale = circleObject.transform.localScale.x;

      if (currentScale < startScale)
      {
        currentScale += shrinkSpeed * Time.deltaTime;
      }

      else
      {
        currentScale = startScale;
      }

      tempscale.Set(currentScale, currentScale, 1.0f);
      circleObject.transform.localScale = tempscale;

      if (audioTimer <= 0)
      {
        GetComponent<AudioSource>().Play();
        audioTimer = 8.0f;
      }
    }
  }

}
