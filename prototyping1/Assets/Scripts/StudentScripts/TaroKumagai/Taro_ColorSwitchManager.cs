using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taro_ColorSwitchManager : MonoBehaviour
{
    public SwitchColor DefaultActiveColor = SwitchColor.Red;
    static public SwitchColor ActiveColor = SwitchColor.Red;

    // Reference for this object 
    static public GameObject Object = null;
    public enum SwitchColor
    { 
        Red,
        Blue,
        Green,
        Yellow, 
        All
    }

    private void Awake()
    {
        Object = this.gameObject;
        ActiveColor = DefaultActiveColor;
    }

    static public SwitchColor GetActiveSwitchColor()
    {
        return ActiveColor;
    }

    static public void SetActiveColor(SwitchColor activeColor)
    {
        ActiveColor = activeColor;
    }

}