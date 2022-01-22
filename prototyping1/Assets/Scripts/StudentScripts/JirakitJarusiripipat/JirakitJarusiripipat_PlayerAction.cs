using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JirakitJarusiripipat_PlayerAction : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField]
    private float skillDuration;
    private float currentSkillDuration;
    [HideInInspector]
    public bool isUsingSkill = false;
    public bool canUseSkill = true;
    [SerializeField]
    private int maxSkillGauge;
    [HideInInspector]
    public int skillGauge;
    [Header("Melee")]
    [HideInInspector]
    public float timeToAttack;
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
    private JirakitJarusiripipat_PlayerMove playermove;
    [Header("SpawnEffect")]
    [SerializeField]
    private GameObject effect1;
    [SerializeField]
    private GameObject effect2;
    [Header("SFX")]
    [SerializeField]
    JirakitJarusiripipat_SFX SFX;
    bool soundCheck = false;


    public LayerMask whatIsEnemies;
    public Text skillGaugeText;
    // Start is called before the first frame update
    void Start()
    {
        defaultTimeToAttackCooldown = timeToAttackCooldown;
        playermove = GetComponent<JirakitJarusiripipat_PlayerMove>();
        defaultPlayerSpeed = playermove.speed;
        SFX.BGM.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && timeToAttack <= 0)
        {
            playermove.anim.SetTrigger("Attack");
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.transform.position, attackRange, whatIsEnemies);
            for (int i = 0; i < enemies.Length; i++)
            {
                if(enemies[i].GetComponent<JirakitJarusiripipat_MonsterMoveHit>() != null)
                {
                    //Debug.Log("Check Collision");
                    enemies[i].GetComponent<JirakitJarusiripipat_MonsterMoveHit>().StopCoroutine("GetHit");
                    enemies[i].GetComponent<JirakitJarusiripipat_MonsterMoveHit>().StartCoroutine("GetHit");
                    if(skillGauge < maxSkillGauge && !isUsingSkill)
                    {
                        skillGauge++;
                    }
                }
                if(enemies[i].GetComponent<JirakitJarusiripipat_ShootMove>() != null)
                {
                    enemies[i].GetComponent<JirakitJarusiripipat_ShootMove>().StopCoroutine("HitEnemy");
                    enemies[i].GetComponent<JirakitJarusiripipat_ShootMove>().StartCoroutine("HitEnemy");
                    if (skillGauge < maxSkillGauge && !isUsingSkill)
                    {
                        skillGauge++;
                        //Debug.Log("SkillGauge = " + skillGauge);
                    }
                }
                
            }
            timeToAttack = timeToAttackCooldown;
        }
        else if(timeToAttack > 0)
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
        //Debug.Log("Skill Gauge = " + skillGauge);
        UpdateSkillGauge();
    }
    private void UpdateSkillGauge()
    {
        //Debug.Log(playerObj.GetComponent<JirakitJarusiripipat_PlayerAction>().skillGauge);
        skillGaugeText.text = skillGauge.ToString();
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

        GameObject obj = Instantiate(effect1, transform.position, Quaternion.identity);
        obj.transform.parent = gameObject.transform;

        SFX.BGM.Pause();
        SFX.Clock1.Play();
        SFX.Skill1.Play();
        soundCheck = false;
    }

    private void AfterSkill()
    {
        isUsingSkill = false;
        canUseSkill = false;
        playermove.speed = defaultPlayerSpeed;
        timeToAttackCooldown = defaultTimeToAttackCooldown;
        SFX.BGM.Play();
        SFX.Clock2.Stop();
        SFX.Skill2.Play();
        GameObject obj = Instantiate(effect1, transform.position, Quaternion.identity);
        obj.transform.parent = gameObject.transform;
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
        if(currentSkillDuration <= 4.0f && !soundCheck)
        {
            SFX.Clock2.Play();
            SFX.Clock1.Stop();
            soundCheck = true;
        }
    }
}
