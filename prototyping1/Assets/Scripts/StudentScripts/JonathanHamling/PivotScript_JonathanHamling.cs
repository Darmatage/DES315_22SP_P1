using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotScript_JonathanHamling : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerPosition;

    private void FixedUpdate()
    {
        this.transform.position = PlayerPosition.transform.position;

        Vector3 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        mousPos.Normalize();

        float rotationZ = Mathf.Atan2(mousPos.y, mousPos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        if (rotationZ < -90 || rotationZ > 90)
        {
            if (PlayerPosition.transform.eulerAngles.y == 0)
            {
                //transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
                this.GetComponentInChildren<SpriteRenderer>().flipY = true;
            }
            else if (PlayerPosition.transform.eulerAngles.y == 180)
            {
                //transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
                this.GetComponentInChildren<SpriteRenderer>().flipY = true;
            }
        }
        else if (rotationZ > -90 || rotationZ < 90)
        {
            if (PlayerPosition.transform.eulerAngles.y == 0)
            {
                //transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
                this.GetComponentInChildren<SpriteRenderer>().flipY = false;
            }
            else if (PlayerPosition.transform.eulerAngles.y == 180)
            {
                //transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
                this.GetComponentInChildren<SpriteRenderer>().flipY = false;
            }
        }
    }
}

