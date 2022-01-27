using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KobeDennis_SignScript : MonoBehaviour
{
    public SpriteRenderer signSprite;
    public Sprite signHighLightSprite;
    private Sprite baseSprite;
    public GameObject signPopUp;
    private GameObject signPopUp_ref;

    [TextArea(3,4)]
    public string signText;
    private void Start()
    {
        baseSprite = signSprite.sprite;
        signPopUp_ref = Instantiate(signPopUp, GameObject.Find("Canvas").transform);
        signPopUp_ref.SetActive(false);
        signPopUp_ref.transform.Find("Text").GetComponent<Text>().text = signText;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            signPopUp_ref.SetActive(true);
            signSprite.sprite = signHighLightSprite;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            signPopUp_ref.SetActive(false);

            signSprite.sprite = baseSprite;
        }
    }
}
