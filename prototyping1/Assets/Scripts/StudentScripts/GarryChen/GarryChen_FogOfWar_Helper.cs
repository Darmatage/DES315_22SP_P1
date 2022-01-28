using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GarryChen_FogOfWar_Helper 
{ 
    public static void ChangeVisionRange(GameObject parent, float scale_x, float scale_y)
    {
        Transform ts = parent.transform;

        foreach(Transform t in ts)
        {
            if(t.tag == "VisionRange")
            {
                t.localScale = new Vector3(scale_x, scale_y, t.localScale.z);
            }
        }
    }
    
}
