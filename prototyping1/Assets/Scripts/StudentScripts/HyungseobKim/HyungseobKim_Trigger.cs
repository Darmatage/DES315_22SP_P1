using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]

public class HyungseobKim_Trigger : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent triggerEvent;

    public GameObject triggerEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            triggerEvent.Invoke();

            Object.Instantiate(triggerEffect, gameObject.transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
