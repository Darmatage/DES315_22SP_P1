using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScottFadoBristow_BootSlimeControl : MonoBehaviour
{
    public float DeathTimer = 10.0f;
    private float t = 255.0f;
    bool active = false;
    private float startingScale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            t -= Time.deltaTime;

            //transform.localScale = new Vector3(0.0f, 0.0f);//new Vector3((t/DeathTimer) * startingScale, (t / DeathTimer) * startingScale, transform.localScale.z);


            if (t <= 0)
            {
                Destroy(gameObject);

            }
        }
    }


    public void Kill()
    {
        active = true;
        t = DeathTimer;
        ParticleSystem ps = GetComponent<ParticleSystem>();//.Play();
        if (ps != null)
            ps.Play();
        startingScale = transform.localScale.x;

        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }
}
