using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MattWalker_HealPlayerOnPickup : MonoBehaviour
{
    public int HealAmount = 25;
    private float DelayTillPickupPossible = 0.5f;
    private float PickupTimer;

    // Start is called before the first frame update
    void Start()
    {
        PickupTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        PickupTimer += Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && PickupTimer >= DelayTillPickupPossible)
        {
            GameHandler gh = FindObjectOfType<GameHandler>();

            if (gh != null)
                gh.Heal(HealAmount);

            Destroy(gameObject);
        }
    }
}
