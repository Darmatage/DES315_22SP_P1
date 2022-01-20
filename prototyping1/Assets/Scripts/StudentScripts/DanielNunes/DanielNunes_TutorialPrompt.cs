using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DanielNunes_TutorialPrompt : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialPanelPrefab;
    private GameObject tutorialPanel;
    [SerializeField]
    private string text;

    private

    // Start is called before the first frame update
    void Start()
    {
        tutorialPanel = Instantiate(tutorialPanelPrefab, GameObject.Find("Canvas").transform);
        tutorialPanel.SetActive(false);

        tutorialPanel.transform.Find("Text").GetComponent<Text>().text = text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tutorialPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tutorialPanel.SetActive(false);
        }
    }
}
