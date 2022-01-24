using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JulianBlackstone_ColorTotemScript : MonoBehaviour
{
    public List<GameObject> colorWavePrefabList = new List<GameObject>();

    public string colorTag = "Red";
    public float defaultWaveExpireTime = 5.0f;
    public float defaultRevealTime = 5.0f;
    public float defaultExpansionVelocity = 3.0f;

    public float defaultCooldown = 5.0f;
    private float cooldownTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (cooldownTimer > 0) return;

        // collided object
        GameObject otherObj = collision.gameObject;

        // check if the colliding object is the player
        if (!otherObj.CompareTag("Player")) return; 

        int colorIndex = -1;

        // set the index in the obj array based on color tag
        switch (colorTag)
        {
            case "Red":
                colorIndex = 0;
                break;
            case "Blue":
                colorIndex = 1;
                break;
            case "Green":
                colorIndex = 2;
                break;
            default:
                return; // fails to find a color on the object, so it returns out of the function entirely
        }
        // create the color wave from correct index at totem position and with identity quaternion for rotation
        GameObject wave = Instantiate(colorWavePrefabList[colorIndex], transform.position, Quaternion.identity);
        
        // set the wave's expansion velocity, object reveal time, and time until the wave expires
        wave.GetComponent<JulianBlackstone_ColorWaveScript>().SetExpansionVelocity(defaultExpansionVelocity);
        wave.GetComponent<JulianBlackstone_ColorWaveScript>().SetRevealTime(defaultRevealTime);
        wave.GetComponent<JulianBlackstone_ExpirationTimer>().SetExpirationTime(defaultWaveExpireTime);

        cooldownTimer = defaultCooldown;

        
    }
}
