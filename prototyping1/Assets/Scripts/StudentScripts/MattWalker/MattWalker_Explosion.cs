using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MattWalker_Explosion : MonoBehaviour
{
    public float ExplosionTime = 0.25f;
    public AudioClip ExplosionSound1;
    public AudioClip ExplosionSound2;
    public AudioClip ExplosionSound3;

    private float LifeTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        int randomClip = Random.Range(0, 3);
        switch (randomClip)
		{
            case 0:
                audio.clip = ExplosionSound1;
                break;
            case 1:
                audio.clip = ExplosionSound2;
                break;
            case 2:
                audio.clip = ExplosionSound3;
                break;
            default:
                audio.clip = ExplosionSound1;
                break;
        }

        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        LifeTimer += Time.deltaTime;
        if (LifeTimer >= ExplosionTime)
            Destroy(gameObject);
    }
}
