using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B21_ProjectileTeleport : MonoBehaviour
{
    /*
     * When using this script the only thing you need to do is drop it into the
     * scene. You can hide it somewhere outside the level so you can't see it.
     *
     * cooldownDuration when set to 0.0f means there is no cooldown.
     *
     * All the values you should care about modifying are exposed onto the
     * editor for ease of use.
     *
     * 
     */
    
    [SerializeField] [Range(1.0f, 10.0f)] private float projectileSpeed = 1.0f;
    [SerializeField] [Range(0.1f, 10.0f)] private float travelDuration = 0.1f;
    [SerializeField] [Range(0.0f, 20.0f)] private float cooldownDuration = 0.0f;
    [SerializeField] private KeyCode shootKeybind = KeyCode.E;
    [SerializeField] private KeyCode cancelShootKeybind = KeyCode.R;

    private GameObject playerObject;
    private GameObject projectile;
    private GameObject teleportObjectPrefab;

    private bool projectileHasBeenShot = false;
    private float projectileDistanceCounter = 0.0f;
    private float cooldownTimer = 0.0f;
    private Vector3 shootDirection;
    private Vector2 lastVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        projectileDistanceCounter = travelDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (projectileHasBeenShot)
        {
            projectileDistanceCounter -= Time.deltaTime;
            if (projectileDistanceCounter <= 0.0f)
            {
                projectileHasBeenShot = false;
                projectileDistanceCounter = travelDuration;
                if (projectile)
                {
                    projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                    projectile.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }

        if (Input.GetKeyDown(cancelShootKeybind))
        {
            if(projectile)
                Destroy(projectile);
        }
        
        if (Input.GetKeyDown(shootKeybind))
        {
            if (!projectileHasBeenShot && projectile == null && cooldownTimer <= 0.0f)
            {
                projectile = Instantiate(this.gameObject, playerObject.transform.position,
                    playerObject.transform.rotation);
                if (projectile)
                {
                    projectileHasBeenShot = true;
                    shootDirection = Input.mousePosition;
                    shootDirection.z = 0.0f;
                    shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
                    shootDirection = shootDirection - projectile.transform.position;

                    projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(shootDirection.x * projectileSpeed,
                        shootDirection.y * projectileSpeed);
                }
            }
            else if (projectile)
            {
                playerObject.transform.position = projectile.transform.position;
                Destroy(projectile);
            }
        }
        
    }
    

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (!other.gameObject.CompareTag("Player") && other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
    //    {
    //        Vector2 reflectedPosition = Vector2.Reflect(gameObject.GetComponent<Rigidbody2D>().velocity.normalized, other.contacts[0].normal);
    //        GetComponent<Rigidbody2D>().velocity = new Vector2(reflectedPosition.x * projectileSpeed,
    //            reflectedPosition.y * projectileSpeed);
    //
    //    }
    //}
}
