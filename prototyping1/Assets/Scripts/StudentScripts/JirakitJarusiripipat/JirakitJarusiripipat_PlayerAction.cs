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
    private float maxSkillGauge;
    [HideInInspector]
    public float skillGauge;
    public GameObject skillBG;
    [Header("Melee")]
    [HideInInspector]
    public float timeToAttack;
    [SerializeField]
    private float timeToAttackCooldown;
    private float defaultTimeToAttackCooldown;
    [SerializeField]
    private Transform rotateParent;
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
    bool isFaceRight = true;

    public LayerMask whatIsEnemies;
    [Header("UI")]
    public Text skillGaugeText;
    public Text[] readyText;
    public Text[] notReadyText;
    public Text[] UsingText;

    public Image cover;
    public Image skillGaugeImage;
    [SerializeField]
    JirakitJarusiripipat_GameManager gameManager;

    [SerializeField]
    private GameObject[] thingsToDestroy;
    [SerializeField]
    private GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        defaultTimeToAttackCooldown = timeToAttackCooldown;
        playermove = GetComponent<JirakitJarusiripipat_PlayerMove>();
        defaultPlayerSpeed = playermove.speed;
        //SFX.BGM.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.S) && isFaceRight)
        {
            rotateParent.eulerAngles = new Vector3(0, 0, -90f);
            
        }
        if (Input.GetKey(KeyCode.S) && !isFaceRight)
        {
            rotateParent.eulerAngles = new Vector3(0, 0, 90f);

        }
        if (Input.GetKey(KeyCode.W) && isFaceRight)
        {
            rotateParent.eulerAngles = new Vector3(0, 0, 90f);
        }
        if (Input.GetKey(KeyCode.W) && !isFaceRight)
        {
            rotateParent.eulerAngles = new Vector3(0, 0, -90f);
        }
        if (Input.GetKey(KeyCode.A) && isFaceRight)
        {
            rotateParent.eulerAngles = new Vector3(0, 0, 180);
            isFaceRight = false;
        }
        if(Input.GetKey(KeyCode.A) && !isFaceRight)
        {
            rotateParent.eulerAngles = new Vector3(0, 0, 0);
            isFaceRight = false;
        }
        if (Input.GetKey(KeyCode.D) && !isFaceRight)
        {
            rotateParent.eulerAngles = new Vector3(0, 0, 0f);
            isFaceRight = true;
        }
        if (Input.GetKey(KeyCode.D) && isFaceRight)
        {
            rotateParent.eulerAngles = new Vector3(0, 0, 0f);
            isFaceRight = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && timeToAttack <= 0)
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
            cover.gameObject.SetActive(true);
        }
        else if(timeToAttack > 0)
        {
            timeToAttack -= Time.deltaTime;
            cover.fillAmount = timeToAttack / timeToAttackCooldown;
        }
        else if(timeToAttack < 0.0f)
        {
            cover.gameObject.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.G) && !isUsingSkill && canUseSkill)
        {
            SkillAttribute();
        }
        if(isUsingSkill)
        {
            CheckDurationSkill();
            for (int i = 0; i < readyText.Length; i++)
            {
                readyText[i].gameObject.SetActive(false);
                notReadyText[i].gameObject.SetActive(false);
                UsingText[i].gameObject.SetActive(true);
            }
        }
        if(skillGauge == maxSkillGauge)
        {
            canUseSkill = true;
        }
        //Debug.Log("Skill Gauge = " + skillGauge);
        UpdateSkillGauge();
        if(canUseSkill && !isUsingSkill)
        {
            for (int i = 0; i < readyText.Length; i++)
            {
                readyText[i].gameObject.SetActive(true);
                notReadyText[i].gameObject.SetActive(false);
                UsingText[i].gameObject.SetActive(false);
            }
        }
        else if(!canUseSkill && !isUsingSkill)
        {
            for (int i = 0; i < readyText.Length; i++)
            {
                readyText[i].gameObject.SetActive(false);
                UsingText[i].gameObject.SetActive(false);
                notReadyText[i].gameObject.SetActive(true);
            }
        }
        skillGaugeImage.fillAmount = skillGauge/maxSkillGauge;
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
        skillBG.SetActive(true);
        GameObject obj = Instantiate(effect1, transform.position, Quaternion.identity);
        obj.transform.parent = gameObject.transform;

        //SFX.BGM.Pause();
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
        //SFX.BGM.Play();
        SFX.Clock2.Stop();
        SFX.Skill2.Play();
        GameObject obj = Instantiate(effect1, transform.position, Quaternion.identity);
        obj.transform.parent = gameObject.transform;
        for (int i = 0; i < readyText.Length; i++)
        {
            readyText[i].gameObject.SetActive(false);
            notReadyText[i].gameObject.SetActive(true);
            UsingText[i].gameObject.SetActive(false);
        }
        skillBG.SetActive(false);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "WallDoor")
        {
            gameManager.hitActive = true;
            gameManager.waveText.gameObject.SetActive(true);
            wall.SetActive(true);
            foreach (var item in thingsToDestroy)
            {
                Destroy(item);
            }
        }
    }
}
