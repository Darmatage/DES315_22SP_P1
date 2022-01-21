using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_ProjectileReflector : MonoBehaviour
{
    [SerializeField] private GameObject m_parent;
    public GameObject Parent { set { m_parent = value; } }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var projectileInfo = collision.gameObject.GetComponent<KennyMecham_ReflectableProjectile>();
        if (!(projectileInfo is null) && projectileInfo.ShouldProjectileBeReflected(m_parent))
        {
            projectileInfo.ReflectTowardsParent(m_parent);
        }
    }
}
