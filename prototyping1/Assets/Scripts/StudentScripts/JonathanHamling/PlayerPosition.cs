using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = Player.transform.position;
    }
}
