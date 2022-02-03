using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BraedanProjectile : MonoBehaviour
{
    public bool isHeld = false;

    private bool particleGo = false;
    private bool isPlaying = false;
    private float particleTime = 1.0f;
    private ParticleSystem particleSystem;
    public GameObject otherParticles;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        var main = particleSystem.main;         
        main.startColor = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        particleSystem.Stop();
    }

    private void Update()
    {
        if(particleGo)
        {
                // Stop it from moving
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                // Hide the art
            transform.GetChild(0).gameObject.SetActive(false);
            particleGo = true;

            // play the particle if not playing 
            if (!isPlaying)
            {
                particleSystem.Play();
                isPlaying = true;
            }
            
                // Destroy the game object once the particles are done
            if (particleTime < 0.0f)
                Destroy(transform.root.gameObject);
            particleTime -= Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHeld)
        {

            // Destoy projectile if it hits a wall
            if(collision.gameObject.name == "TilemapWalls")
                particleGo = true;
                
            // Destroy the projectile and any hit enemy
            else if(collision.gameObject.layer == 6)
            {
                particleGo = true;
                ParticleSystem otherPart = otherParticles.GetComponent<ParticleSystem>();
                var main = otherPart.main;
                main.startColor = collision.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                Instantiate(otherParticles, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
            }

            // Destroy the tile for the breakable wall
            else if(collision.gameObject.CompareTag("BraedanNeversBreakable"))
            {
                // Destroy the projectile and wall
                particleGo = true;
                ParticleSystem otherPart = otherParticles.GetComponent<ParticleSystem>();
                var main = otherPart.main;
                main.startColor = collision.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                Instantiate(otherParticles, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
            }
        }   
        
    }
}
