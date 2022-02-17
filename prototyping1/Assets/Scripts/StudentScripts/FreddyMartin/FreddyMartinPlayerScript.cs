using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreddyMartinPlayerScript : MonoBehaviour
{
    public GameObject CooldownPrefab;

    public float ForceSwapCooldown = 5.0f;

    static public FreddyMartinPlayerScript Instance;

    [HideInInspector]
    public bool forceSwap = false;

    float forceSwapTimer = 0.0f;

    GameObject cooldownBar;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        cooldownBar = Instantiate(CooldownPrefab, transform);
        cooldownBar.transform.localScale = new Vector3(0, 0.2f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        forceSwap = false;

        if (forceSwapTimer > 0)
        {
            forceSwapTimer -= Time.deltaTime;
            forceSwapTimer = Mathf.Max(0, forceSwapTimer);
        }

        if (forceSwapTimer <= 0 && Input.GetKeyDown(KeyCode.E))
        {
            forceSwapTimer = ForceSwapCooldown;

            forceSwap = true;
        }

        cooldownBar.transform.localScale = new Vector3(forceSwapTimer / ForceSwapCooldown, 0.2f, 1);
    }
}
