using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaeunJeong_Pickups : MonoBehaviour
{
    public enum PICKUP_TYPE
    {
        NONE,
        COIN,
        HEALTH,
        JEWEL
    };

    public PICKUP_TYPE pickupType;
    private GameObject chest;
    private DaeunJeong_UIManager UIManager;
    private WonjuJo_PlayerHandler wonjuPlayerHandler;
    private EunjinHong_GameHandler eunjinPlayerHandler;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "Coin")
        {
            pickupType = PICKUP_TYPE.COIN;
        }
        else if (gameObject.tag == "Health")
        {
            pickupType = PICKUP_TYPE.HEALTH;
            GameObject gameHandlerCanvas = GameObject.Find("WonjuJo_GameHandlerCanvas");

            if (gameHandlerCanvas == null)
            {
                gameHandlerCanvas = GameObject.Find("GameHandlerCanvas");
                eunjinPlayerHandler = gameHandlerCanvas.gameObject.GetComponentInChildren<EunjinHong_GameHandler>();
            }
            else
            {
                wonjuPlayerHandler = gameHandlerCanvas.gameObject.GetComponentInChildren<WonjuJo_PlayerHandler>();
            }
        }
        else if (gameObject.tag == "Jewel")
        {
            pickupType = PICKUP_TYPE.JEWEL;
        }

        chest = GameObject.FindGameObjectWithTag("MysteriousChest");

        if (chest != null)
        {
            DaeunJeong_MysteriousChest mysteriousChest = chest.GetComponent<DaeunJeong_MysteriousChest>();

            if (mysteriousChest != null)
            {
                UIManager = mysteriousChest.UIManager;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UIManager.ShowUITextForPickups(pickupType);

            if (pickupType == PICKUP_TYPE.HEALTH)
            {
                if (wonjuPlayerHandler != null)
                {
                    wonjuPlayerHandler.Heal(10);
                }
                else if (eunjinPlayerHandler != null)
                {
                    eunjinPlayerHandler.Heal(10);
                }
            }

            Destroy(gameObject);
        }
    }
}