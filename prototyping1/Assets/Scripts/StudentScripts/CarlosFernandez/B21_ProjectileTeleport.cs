using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B21_ProjectileTeleport : MonoBehaviour
{
    /*
     * When using this script the only thing you need to assign is the
     * "teleportPrefab" which is called B21_TeleportPrefab.
     *
     * cooldownDuration when set to 0.0f means there is no cooldown.
     *
     * All the values you should care about modifying are exposed onto the
     * editor for ease of use.
     */
    [SerializeField] private GameObject teleportObjectPrefab;
    [SerializeField] [Range(1.0f, 10.0f)] private float projectileSpeed = 1.0f;
    [SerializeField] [Range(0.1f, 10.0f)] private float travelDuration = 0.1f;
    [SerializeField] [Range(0.0f, 20.0f)] private float cooldownDuration = 0.0f;
    [SerializeField] private KeyCode shootKeybind = KeyCode.E;

    private GameObject playerObject;
    private GameObject projectile;
    
    private bool projectileHasBeenShot = false;
    private float projectileDistanceCounter = 0.0f;
    private float cooldownTimer = 0.0f;
    
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
                if(projectile)
                    projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            }
        }
        
        if (Input.GetKeyDown(shootKeybind))
        {
            if (!projectileHasBeenShot && projectile == null && cooldownTimer <= 0.0f)
            {
                projectile = Instantiate(teleportObjectPrefab, playerObject.transform.position,
                    playerObject.transform.rotation);
                if (projectile)
                {
                    projectileHasBeenShot = true;
                    Vector3 shootDirection = Input.mousePosition;
                    shootDirection.z = 0.0f;
                    shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
                    shootDirection = shootDirection - transform.position;

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
}
