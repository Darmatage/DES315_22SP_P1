using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BenMowry_HealthPickupScriptNorm : MonoBehaviour
{
    GameHandler gameHandler;
    AudioSource audioData;
    Renderer rend;
    BoxCollider2D box;

    public int healthAmount = 0;

    void Awake()
    {
        gameHandler = FindObjectOfType<GameHandler>();
        audioData = GetComponent<AudioSource>();
        rend = this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(GameHandler.PlayerHealth < gameHandler.PlayerHealthStart)
            {
                audioData.Play(0);
                rend.enabled = false;
                box.enabled = false;
                Destroy(gameObject, audioData.clip.length);
                gameHandler.Heal(healthAmount);
            }
        }
    }
}
