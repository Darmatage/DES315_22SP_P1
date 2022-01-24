using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenThompson_BoatControlSceneHandler : MonoBehaviour
{
    private void OnDestroy()
    {
        // Inform the boat controller script that we are going to a new scene and
        // as such we need to show the UI again.
        BenThompson_BoatController.GoingToNewScene();
    }
}
