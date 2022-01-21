using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript_JonathanHamling : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    //[SerializeField]
    private bool isHeld = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isHeld = true;

            if (isHeld)
                isHeld = true;
        }
    }

}
