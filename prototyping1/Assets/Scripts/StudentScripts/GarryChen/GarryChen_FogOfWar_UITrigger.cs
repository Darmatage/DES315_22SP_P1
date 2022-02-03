using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarryChen_FogOfWar_UITrigger : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject UIPanelPrefab;
    public DaeunJeong_UIManager_GarryChenModified UIManager;
    public string TextShowed;
    public float TextMaxTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        bool isThereUIPanel = false;

        if (UIPanelPrefab != null && Canvas != null)
        {
            for (int i = 0; i < Canvas.gameObject.transform.childCount; ++i)
            {
                if (Canvas.gameObject.transform.GetChild(i).gameObject.GetComponent<DaeunJeong_UIManager_GarryChenModified>())
                {
                    isThereUIPanel = true;
                    UIManager = Canvas.gameObject.transform.GetChild(i).gameObject.GetComponent<DaeunJeong_UIManager_GarryChenModified>();
                }
            }

            if (!isThereUIPanel)
            {
                GameObject UIPanel = Instantiate(UIPanelPrefab, Canvas.transform);
                UIManager = UIPanel.GetComponent<DaeunJeong_UIManager_GarryChenModified>();
            }
        }
        else
        {
            Debug.Log("[DaeunJeon] You need to assign UI Panel prefab and Canvas.");
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            UIManager.ShowUIText(TextShowed);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            UIManager.SetStopShowingUIPanel(TextMaxTime);
        }
    }
}
