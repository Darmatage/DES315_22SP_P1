using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_CombatStats : MonoBehaviour
{
  public delegate void OnDeath(GameObject dead);
  public delegate void OnDamage(int damage);

  public OnDeath onDeath;
  public OnDamage onDamage;

  public int maxHealth;
  public int damage;
  public int speed;
  public int lives;
  public float attackCooldown;

  private int m_health;

  public virtual void OnStart() { }
  public void Start()
  {
    m_health = maxHealth;
    OnStart();
  }
  public bool IsDead()
  {
    return m_health <= 0;
  }
  public void Damage(int damage)
  {
    if (!IsDead())
    {
       m_health -= damage;
       if(!(onDamage is null))
        onDamage(damage);

      if (IsDead())
      {
        gameObject.SetActive(false);

        if(!(onDeath is null))
          onDeath(gameObject);
      }
    }
  }
}
