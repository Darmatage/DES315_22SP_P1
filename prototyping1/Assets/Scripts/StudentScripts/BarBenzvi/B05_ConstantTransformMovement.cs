using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_ConstantTransformMovement : MonoBehaviour
{
    public Vector3 Velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.position += Velocity * Time.unscaledDeltaTime;
    }
}
