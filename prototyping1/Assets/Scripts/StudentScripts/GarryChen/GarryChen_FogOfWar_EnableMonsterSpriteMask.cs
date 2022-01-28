using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarryChen_FogOfWar_EnableMonsterSpriteMask : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] MonsterSprites;
    void Start()
    {
        MonsterSprites = GameObject.FindGameObjectsWithTag("MonsterArt");

        foreach (GameObject Sprite in MonsterSprites)
        {
            Sprite.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
