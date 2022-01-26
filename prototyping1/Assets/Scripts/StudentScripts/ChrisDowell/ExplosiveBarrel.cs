using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    // Start is called before the first frame update
    public enum ExplosiveTrigger
    {
        Damage,
        NoHp,
        Touch,
        Manual,

    }

    [System.Serializable]
    public class BarrelGameData
    {
        [Header("Timer")]
        public float m_timerLength = 2f;
        [System.NonSerialized] public float m_dt = 0;
        [System.NonSerialized] public int m_hp = 10;
        [Header("Statistics")]
        public int m_maxHP = 10;
        public int m_damage = 25;
        public float m_explosiveRadius = 2f;
        public float m_pushforce = 1;
        [System.NonSerialized] public bool m_exploded;
    }

    [System.Serializable]
    public class BarrelVisualData
    {
        public Gradient m_barrelColors;
        public Vector2 m_newScale;
        public Gradient m_indicatorColor;
    }

    public ExplosiveTrigger m_triggerType = 0;
    public BarrelGameData m_stats;
    public BarrelVisualData m_visuals;
    public LayerMask m_damageMask;
    
    public bool m_triggered = false;

    //public LayerMask m_deleteMask; // Deletes objects if explodes
    public SpriteRenderer m_indicator;
    private CircleCollider2D m_exlosivetrigger;
    private GameHandler m_handler;
    private Vector3 m_initscale = Vector3.one;
    private Vector3 m_newScale;
    private SpriteRenderer m_renderer;
    private Rigidbody2D m_rb;
    private float m_radius = 0;
    private float xplodedt = 0;
    private float m_startRadius = .1f;
    private float m_explodeTime = .3f;

    private void Awake()
    {
        m_initscale = transform.localScale;
        m_newScale = transform.localScale * m_visuals.m_newScale;
        
    }

    void Start()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_rb = GetComponent<Rigidbody2D>();
        m_handler = FindObjectOfType<GameHandler>();
        m_exlosivetrigger = GetComponent<CircleCollider2D>();
        m_exlosivetrigger.enabled = false;
        m_radius = m_exlosivetrigger.radius;
        m_startRadius = m_exlosivetrigger.radius / 4;

    }

    // Update is called once per frame
    void Update()
    {
        ref int health = ref m_stats.m_hp;
        int maxhp = m_stats.m_maxHP;

        //if (m_stats.m_exploded)
        //{
        //    return;
        //}

        if (health < maxhp && (m_triggerType != ExplosiveTrigger.Manual || m_triggerType != ExplosiveTrigger.NoHp))
        {
            m_triggered = true;
        }

        // Barrel will always explode when no hp
        if (health == 0)
        {
            Explode();
        }

        if (m_triggered)
        {
            m_stats.m_dt = Mathf.Clamp(m_stats.m_dt + Time.deltaTime, 0, m_stats.m_timerLength);

            if (m_stats.m_dt >= m_stats.m_timerLength)
            {
                // Trigger explosion
                Explode();
            }
            else
            {
                // Do vfx
                float t = m_stats.m_dt / m_stats.m_timerLength;
                m_renderer.color = m_visuals.m_barrelColors.Evaluate(t);
                transform.localScale = Vector3.Lerp(m_initscale, m_newScale, t);
                m_indicator.color = m_visuals.m_indicatorColor.Evaluate(t);


            }
        }

        if (m_stats.m_exploded == true)
        {
            xplodedt = Mathf.Clamp(xplodedt + Time.deltaTime, 0, m_explodeTime);
            float t = xplodedt / m_explodeTime;
            m_exlosivetrigger.radius = Mathf.Lerp(m_startRadius, m_radius, t);

            
        }





    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (m_triggerType == ExplosiveTrigger.Touch)
        {

            TakeDamage(1);
        }

        //if (collision.collider.GetType().ToString() == "BoxCollider2D")
        //{
        //}
        //else if (collision.collider.GetType() == typeof(CircleCollider2D))
        //{
        //    // Deal Damage
        //    if (collision.otherCollider.tag == "Player")
        //    {
        //        m_handler.TakeDamage(m_stats.m_damage);
        //    }
        //}
    }


    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
    private void Explode()
    {
        m_stats.m_exploded = true;

        // Take everythign in the radius and deal damage and push them back

        Vector2 dir = m_rb.velocity.normalized;
        m_exlosivetrigger.enabled = true;
        m_exlosivetrigger.radius = m_startRadius;
        m_rb.mass = 100f; // Set mass so it wont move lmao
        m_rb.drag = 100f;
        m_rb.constraints = RigidbodyConstraints2D.FreezePosition;

        Invoke("DestroySelf", m_explodeTime);

        //var hit = Physics2D.CircleCastAll(transform.position, m_stats.m_explosiveRadius, dir, 0, m_damageMask, Mathf.NegativeInfinity, Mathf.Infinity);

        //foreach (var obj in hit)
        //{
        //    // Get vector from object ot target
        //    obj.rigidbody.AddForce(obj.normal * m_stats.m_pushforce);

        //    if (obj.collider.tag == "Player")
        //    {
        //        m_handler.TakeDamage(m_stats.m_damage);
        //        continue;
        //    }


        //    // Get come component script and add it here to deal damage since we dont have some externalized health script?!?!?!!?
        //    // TODO: figure out how to damage multiple enemies with DIFFERING SCRIPTS LIKE HOLY SHIT WHY
        //    if (obj.collider.tag == "Enemy")
        //    {
        //        // I gotta somehow adjust their hp lol

        //        // I will add some damage stuff later
        //        // If you end up using my prefab please let me know on teams (c.dowell@digipen.edu) or discord (Frost#0006)
        //        // DISCORD IS PREFERRED IM MOST LIKELY GOING TO IGNORE TEAMS


        //    }

        //}

        //GameObject.Destroy(gameObject);



    }

    /// <summary>
    /// Causes the barrel to take a certain amount of damage
    /// </summary>
    /// <param name="damage">The amount of points of damage it should recieve</param>
    /// <returns>The current health of the barrel</returns>
    public int TakeDamage(int damage)
    {
        m_stats.m_hp = Mathf.Clamp(m_stats.m_hp - damage, 0, m_stats.m_maxHP);
        return m_stats.m_hp;
    }


    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
