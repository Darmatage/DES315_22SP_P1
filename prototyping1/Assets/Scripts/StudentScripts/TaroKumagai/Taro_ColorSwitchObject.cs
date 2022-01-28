using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Taro_ColorSwitchObject : MonoBehaviour
{
    public Taro_ColorSwitchManager.SwitchColor SwitchColor = Taro_ColorSwitchManager.SwitchColor.Red;
    public float InactiveAlpha = 0.3f;
    public SpriteRenderer sprite;
    public Tilemap tilemap;


    private void OnCollisionTrigger(Collision2D collision)
    {
        var possibleProjectile = collision.gameObject;
        Projectile projectile = possibleProjectile.GetComponent<Projectile>();

        // Destroy the projectile if the color objectr is active
        if (isActive() && projectile != null)
            Destroy(possibleProjectile);
    }

    public bool isActive()
    {
        return SwitchColor == Taro_ColorSwitchManager.GetActiveSwitchColor();
    }

    private void Update()
    {
        // Active State
        if (isActive())
        {
            // Will now collide with the player
            gameObject.layer = LayerMask.NameToLayer("Enemy");

            // Making the object opaque
            if (sprite)
            {
                Color color = sprite.color;
                color.a = 1.0f;
                sprite.color = color;
            }
            else if (tilemap)
            {
                Color color = tilemap.color;
                color.a = 1.0f;
                tilemap.color = color;
            }

        }
        // Inactive State
        else
        {
            // No longer collides with the player
            gameObject.layer = LayerMask.NameToLayer("IgnorePlayerAndEnemy");

            
            // Making the object opaque
            if (sprite)
            {
                Color color = sprite.color;
                color.a = InactiveAlpha;
                sprite.color = color;
            }
            else if (tilemap)
            {
                Color color = tilemap.color;
                color.a = InactiveAlpha;
                tilemap.color = color;
            }
        }
    }

}