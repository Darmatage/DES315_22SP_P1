using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KennyMecham_ReflectableAttackBase : MonoBehaviour
{
  [SerializeField] protected int damage_;
  [SerializeField] protected float lifetime_;
  protected float current_lifetime_;
  protected GameObject target_;
  protected GameObject attacker_;

  public float Damage { get => damage_;  }
  public float MaxLifetime { get => lifetime_; }
  public float CurrentLifetime { get => current_lifetime_; }
  public GameObject Attacker { get => attacker_; }
  public GameObject Target { get => target_; }

  protected abstract void OnAttack(GameObject target);
  protected abstract void OnReflect(GameObject reflector);
  protected abstract void OnHit();
  protected abstract void OnDeath();
  protected abstract bool ShouldHit(GameObject obj);

  public void Attack(GameObject attacker, GameObject target)
  {
    attacker_ = attacker;
    target_ = target;
    current_lifetime_ = lifetime_;
    OnAttack(target);
  }

  public void Reflect(GameObject reflector)
  {
    OnReflect(reflector);
  }

  public void FixedUpdate()
  {
    if(current_lifetime_ > 0)
    {
      current_lifetime_ -= Time.fixedDeltaTime;

      if(current_lifetime_ <= 0)
      {
        current_lifetime_ = 0;
        OnDeath();
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if(ShouldHit(collision.gameObject))
    {
      OnHit();
    }
  }
}
