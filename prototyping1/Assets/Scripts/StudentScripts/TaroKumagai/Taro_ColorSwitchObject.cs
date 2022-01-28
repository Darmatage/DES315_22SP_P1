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

    
    private void Update()
    {
        // Active State
        if (SwitchColor == Taro_ColorSwitchManager.GetActiveSwitchColor())
        {
            // Will now collide with the player
            gameObject.layer = LayerMask.NameToLayer("Default");

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
            gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");

            
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