using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_CombatStats : MonoBehaviour
{
    public delegate void OnDeath(GameObject dead);
    public OnDeath onDeath;

    public int maxHealth;
    public int damage;
    public int speed;
    public int lives;
    public float attackCooldown;

    private int m_health;

    private void Start()
    {
        m_health = maxHealth;
    }
    public bool IsDead()
    {
        return m_health <= 0;
    }
    public void Damage(int damage)
    {
        if(!IsDead())
        {
            m_health -= damage;

            if(IsDead())
            {
                onDeath(gameObject);
            }
        }
    }
}
