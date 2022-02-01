using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class B03_Trigger : MonoBehaviour
{
    public delegate void FUNCTION();
    public FUNCTION OnEnter = null;
    public FUNCTION OnExit = null;

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnExit();
        }
    }
}
