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
        [System.NonSerialized] public float m_dt = 0;
        [System.NonSerialized] public int m_hp = 10;
        [Header("Statistics")]
        public int m_maxHP = 10;
        public int m_damage = 25;
        public float m_explosiveRadius = 1f;
        private bool m_exploded;
    }

    [System.Serializable]
    public class BarrelVisualData
    {
        [System.NonSerialized] public Color m_initColor = Color.red;
        public Color m_tickColor = Color.white;
        [Header("Timer")]
        public float m_tickInterval = .5f;
        public float m_tickLength = .1f;
        [System.NonSerialized] private float m_tickdt = 0;
        [System.NonSerialized] public bool Tick;
    }

    public BarrelGameData m_stats;
    public BarrelVisualData m_visuals;

    private SpriteRenderer m_renderer;
    private Rigidbody2D m_rb;

    private void Awake()
    {
    }

    void Start()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
