using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taro_ProjectileAddOn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       GameObject hit = collision.gameObject;

        if (hit.tag == "Taro_ColorBlock")
            Destroy(gameObject);
    }
}
