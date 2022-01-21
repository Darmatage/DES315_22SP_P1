using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_ReflectableProjectile : MonoBehaviour
{
    private GameObject m_parent;
    private GameObject m_target;
    private GameHandler m_gameHandler;
    private float lifeTimer;
    [SerializeField] private GameObject hitAnimation;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage;
    // Start is called before the first frame update
    void Start()
    {
        m_parent = gameObject.gameObject;
        GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
        if (gameHandlerLocation != null)
        {
            m_gameHandler = gameHandlerLocation.GetComponent<GameHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_target.transform.position, speed * Time.deltaTime);
    }

    public void FireProjectile(GameObject target, GameObject parent)
    {
        lifeTimer = lifetime;
        m_parent = parent;
        m_target = target;
    }

    public void ReflectTowardsParent(GameObject newParent)
    {
        FireProjectile(m_parent, newParent);
    }

    public bool ShouldProjectileBeReflected(KennyMecham_ProjectileReflector reflector)
    {
        var reflectorTeams = reflector.gameObject.GetComponent<KennyMecham_Teams>();

        // if the reflector doesn't have any teams, or if the reflector and this do not share any teams
        if (reflectorTeams is null || !reflectorTeams.DoTeamsOverlap(m_parent.GetComponent<KennyMecham_Teams>()))
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        KennyMecham_Teams teams = other.gameObject.GetComponent<KennyMecham_Teams>();
        if (teams is null || !teams.DoTeamsOverlap(m_parent.GetComponent<KennyMecham_Teams>()))
        {
            m_gameHandler.TakeDamage(damage);
            GameObject animEffect = Instantiate(hitAnimation, transform.position, Quaternion.identity);
            Destroy(animEffect, 0.5f);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        lifeTimer -= Time.fixedDeltaTime;

        if(lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
