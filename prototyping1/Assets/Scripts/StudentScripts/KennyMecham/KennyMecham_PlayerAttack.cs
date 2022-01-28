using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_PlayerAttack : MonoBehaviour
{
  public KeyCode attack_key_;
  private KennyMecham_ReflectorBase reflector_;
  // Start is called before the first frame update
  void Start()
  {
    reflector_ = GetComponent<KennyMecham_ReflectorBase>();
  }

  // Update is called once per frame
  void Update()
  {
    if(Input.GetKeyDown(attack_key_))
    {
      reflector_.ActivateDetectors();
    }
    else if(Input.GetKeyUp(attack_key_))
    {
      reflector_.DeactivateDetectors();
    }
  }
}
