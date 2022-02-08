using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_PlayerStats : KennyMecham_CombatStats
{
  private GameHandler game_handler;
  // Start is called before the first frame update
  public override void OnStart()
  {
    game_handler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
    onDamage += TakeDamage;
  }

  void TakeDamage(int damage)
  {
    game_handler.TakeDamage(damage);
  }
}
