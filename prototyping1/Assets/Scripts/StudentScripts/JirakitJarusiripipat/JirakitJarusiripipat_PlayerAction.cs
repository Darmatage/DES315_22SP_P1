using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JirakitJarusiripipat_PlayerAction : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField]
    private float skillDuration;
    private float currentSkillDuration;
    private bool isUsingSkill = false;
    public bool canUseSkill = true;
    [SerializeField]
    private int maxSkillGauge;
    private int skillGauge;
    [Header("Melee")]
    private float timeToAttack;
    [SerializeField]
    private float timeToAttackCooldown;
    private float defaultTimeToAttackCooldown;
    [SerializeField]
    private Transform attackPos;
    [SerializeField]
    private float attackRange;
    [Header("Attribute")]
    private float defaultPlayerSpeed;
    [SerializeField]
    private float skillPlayerSpeed;
    private PlayerMove playermove;

    public LayerMask whatIsEnemies;
    List<GameObject> allEnemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        defaultTimeToAttackCooldown = timeToAttackCooldown;
        playermove = GetComponent<PlayerMove>();
        defaultPlayerSpeed = playermove.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && timeToAttack <= 0)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.transform.position, attackRange, whatIsEnemies);
            for (int i = 0; i < enemies.Length; i++)
            {
                if(enemies[i].GetComponent<MonsterMoveHit>() != null)
                {
                    //Debug.Log("Check Collision");
                    enemies[i].GetComponent<MonsterMoveHit>().StopCoroutine("GetHit");
                    enemies[i].GetComponent<MonsterMoveHit>().StartCoroutine("GetHit");
                    if(skillGauge < maxSkillGauge && !isUsingSkill)
                    {
                        skillGauge++;
                    }
                }
                //if(enemies[i].GetComponent<MonsterShootMove>() != null)
                //{
                //    enemies[i].GetComponent<MonsterShootMove>().StopCoroutine("GetHit");
                //    enemies[i].GetComponent<MonsterShootMove>().StartCoroutine("GetHit");
                //    if (skillGauge < maxSkillGauge && !isUsingSkill)
                //    {
                //        skillGauge++;
                //    }
                //}
                
            }
            timeToAttack = timeToAttackCooldown;
        }
        else
        {
            timeToAttack -= Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.G) && !isUsingSkill && canUseSkill)
        {
            SkillAttribute();
        }
        if(isUsingSkill)
        {
            CheckDurationSkill();
        }
        if(skillGauge == maxSkillGauge)
        {
            canUseSkill = true;
        }
        Debug.Log("Skill Gauge = " + skillGauge);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.transform.position, attackRange);
    }
    private void SkillAttribute()
    {
        isUsingSkill = true;
        currentSkillDuration = skillDuration;
        playermove.speed = skillPlayerSpeed;
        timeToAttackCooldown = 0.3f;
        skillGauge = 0;
    }

    private void AfterSkill()
    {
        isUsingSkill = false;
        canUseSkill = false;
        playermove.speed = defaultPlayerSpeed;
        timeToAttackCooldown = defaultTimeToAttackCooldown;
    }

    private void CheckDurationSkill()
    {
        if (currentSkillDuration > 0.0f)
        {
            currentSkillDuration -= Time.deltaTime;
        }
        else if (currentSkillDuration <= 0.0f)
        {
            AfterSkill();
        }
    }
}
