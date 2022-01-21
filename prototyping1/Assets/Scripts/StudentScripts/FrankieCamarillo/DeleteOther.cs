using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOther : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // object is in enemy layer
            if (contact.otherCollider.gameObject.layer == 6)
                Destroy(contact.otherCollider.gameObject);
        }
    }
}
