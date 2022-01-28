using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_ReflectableProjectile : KennyMecham_ReflectableAttackBase
{
  private Vector2 m_target_direction;
  private GameHandler m_gameHandler = null;
  [SerializeField] private GameObject hit_animation;
  [SerializeField] private float speed;
  private List<string> tags_to_hit_;

  // Start is called before the first frame update
  void Start()
  {
    GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
    if (gameHandlerLocation != null)
    {
      m_gameHandler = gameHandlerLocation.GetComponent<GameHandler>();
    }
  }
  // Update is called once per frame
  void Update()
  {
    Vector2 position = new Vector2(transform.position.x, transform.position.y);
    transform.position = position + m_target_direction * speed * Time.deltaTime;
  }
  protected override void OnAttack(GameObject target)
  {
    tags_to_hit_ = COTags.GetAllGameObjectTags(target);
    m_target_direction = target.transform.position - Attacker.transform.position;
  }
  protected override void OnReflect(GameObject reflector)
  {
    current_lifetime_ = lifetime_;
   
    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    m_target_direction = mousePos - reflector.transform.position;
    target_ = attacker_;
    attacker_ = reflector;
    current_lifetime_ = lifetime_;
    tags_to_hit_ = COTags.GetAllGameObjectTags(target_);
  }
  protected override void OnDeath() 
  {
    GameObject animEffect = Instantiate(hit_animation, transform.position, Quaternion.identity);
    Destroy(animEffect, 0.5f);
    Destroy(gameObject);
  }
  protected override void OnHit()
  {
    var combatStats = Target.GetComponent<KennyMecham_CombatStats>();
    if (!(combatStats is null))
    {
      combatStats.Damage(damage_);
    }

    GameObject animEffect = Instantiate(hit_animation, transform.position, Quaternion.identity);
    Destroy(animEffect, 0.5f);
    Destroy(gameObject);
  }
  protected override bool ShouldHit(GameObject obj)
  {
    return COTags.CompareAnyTags(obj, tags_to_hit_);
  }
}
