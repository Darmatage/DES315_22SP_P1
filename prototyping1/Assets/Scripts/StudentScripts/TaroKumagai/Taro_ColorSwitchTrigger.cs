using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component that can be attached to any object to make it so they can trigger a color switch
/// Just Attach this component and decide whether or not its active. 
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Taro_ColorSwitchTrigger : MonoBehaviour
{
    // Flag for whether this object will trigger the switch
    public bool isActive = true;

    // Flag what colors switches this object can trigger.
    public Taro_ColorSwitchManager.SwitchColor SwitchColor = Taro_ColorSwitchManager.SwitchColor.All;
}