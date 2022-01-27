using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class Taro_ColorSwitchBehavior : MonoBehaviour
{
    public Taro_ColorSwitchManager.SwitchColor SwitchColor = Taro_ColorSwitchManager.SwitchColor.Red;


    public GameObject ActiveSprite;
    public GameObject InactiveSprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var switchTrigger = other.GetComponent<Taro_ColorSwitchTrigger>();

        if (switchTrigger != null && switchTrigger.isActive)
            Taro_ColorSwitchManager.SetActiveColor(SwitchColor);

    }

    private void Update()
    {
        if (SwitchColor == Taro_ColorSwitchManager.ActiveColor)
        {
            //get all cannons in the scene
            DanielNunes_Cannon[] cannons = FindObjectsOfType<DanielNunes_Cannon>();

            //go through all cannons and...
            for (int i = 0; i < cannons.Length; ++i)
            {
                //...briefly reset each of their raycast contacts
                cannons[i].ResetContacts();
            }

            ActiveSprite.SetActive(true);
            InactiveSprite.SetActive(false);
        }
        else
        {
            ActiveSprite.SetActive(false);
            InactiveSprite.SetActive(true);
        }
    }

}