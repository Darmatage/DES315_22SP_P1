using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    // Start is called before the first frame update

    [System.Serializable]
    public class BarrelGameData
    {
        [Header("Timer")]
        public float m_timerLength = 2f;
        [System.NonSerialized] public bool m_triggered = false;
        [System.NonSerialized] public float m_dt = 0;
        [System.NonSerialized] public int m_hp = 10;
        [Header("Statistics")]
        public int m_maxHP = 10;
        public int m_damage = 25;
        public float m_explosiveRadius = 1f;
        public float m_pushforce = 1;
        [System.NonSerialized] public bool m_exploded;
    }

    [System.Serializable]
    public class BarrelVisualData
    {
        public Gradient m_barrelColors;
        public Vector2 m_newscale;
    }

    public BarrelGameData m_stats;
    public BarrelVisualData m_visuals;


    private Vector3 m_initscale = Vector3.one;
    private Vector3 m_newScale;
    private SpriteRenderer m_renderer;
    private Rigidbody2D m_rb;

    private void Awake()
    {
        m_initscale = transform.localScale;
        m_newScale = m_visuals.m_newscale;
    }

    void Start()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ref int health = ref m_stats.m_hp;
        int maxhp = m_stats.m_maxHP;

        if (m_stats.m_exploded)
        {
            return;
        }

        if (health < maxhp)
        {
            m_stats.m_triggered = true;
        }

        if (health == 0)
        {
            Explode();
        }

        if (m_stats.m_triggered)
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
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
         
    }

    private void Explode()
    {
        m_stats.m_exploded = true;

        // Take everythign in the radius and deal damage and push them back

        var hit = Physics2D.CircleCastAll(transform.position, m_stats.m_explosiveRadius, Vector2.zero);
        
        foreach (var obj in hit)
        {

        }



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
}
