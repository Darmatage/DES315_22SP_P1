using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_ProjectileReflector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var projectileInfo = collision.gameObject.GetComponent<KennyMecham_ReflectableProjectile>();
        if (!(projectileInfo is null) && projectileInfo.ShouldProjectileBeReflected(this))
        {
            projectileInfo.ReflectTowardsParent(gameObject);
        }
    }
}
