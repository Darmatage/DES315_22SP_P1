using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]

public class HyungseobKim_Trigger : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent triggerEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            triggerEvent.Invoke();

            Destroy(gameObject);
        }
    }
}
