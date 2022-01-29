using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KickWhoosh : MonoBehaviour
{
    public float Distance;
    public float Speed;
    public float Size;
    public float Damage;

    private Vector3 SpawnLocation;

    void Start()
    {
        SpawnLocation = transform.localPosition;
        transform.localScale = new Vector3(Size, Size);
    }

    void FixedUpdate()
    {
        transform.localPosition += transform.right * Speed * Time.fixedDeltaTime;
        if (transform.localPosition.magnitude >= Distance)
            Destroy(gameObject);
    }
    public void SetPower(float power)
    {
        Distance = 1.2f * (1f + power);
        Size = 0.8f * (1f + power);
        Speed = 7f * (1f + power);
        Damage = 1f * power;
    }
}
