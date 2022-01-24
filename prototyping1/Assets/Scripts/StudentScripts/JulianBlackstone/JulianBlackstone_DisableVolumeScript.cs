using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JulianBlackstone_DisableVolumeScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> wallsToApply = new List<GameObject>();


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        foreach (GameObject currWall in wallsToApply)
        {
            JulianBlackstone_ColorSystem currColorSys = currWall.GetComponent<JulianBlackstone_ColorSystem>();

            if (currColorSys == null) continue; // skip if the object does not have a valid color system

            currColorSys.ActivateColorEffect(5.0f);
        }

    }
}
