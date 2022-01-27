using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MattWalker_CoinPickup : MonoBehaviour
{
    public int PointValue = 10;
    private float DelayTillPickupPossible = 0.5f;
    private float PickupTimer;

    private MattWalker_PlayerScore scoreScript;

    // Start is called before the first frame update
    void Start()
    {
        GameObject ScoreUI = GameObject.Find("MattWalker_PlayerScoreUI");
        if (ScoreUI != null)
            scoreScript = ScoreUI.GetComponent<MattWalker_PlayerScore>();

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
            if (scoreScript != null)
                scoreScript.IncreaseScore(PointValue);

            Destroy(gameObject);
        }
    }
}
