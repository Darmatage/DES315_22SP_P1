using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HemieChoi_Boomerang : MonoBehaviour
{
    private float rotationSpeed;
    private float movingSpeed;
    private bool isRotating;
    private Vector3 targetPos;
    private bool isThrowing;
    private bool isDropped;
    [SerializeField] private bool isHolding;
    private GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 1000.0f;
        movingSpeed = 5.0f;
        playerObj = GameObject.Find("Player");
        isHolding = true;
    }

    // Update is called once per frame
    void Update()
    {
        RotateWeapon();

        if (isHolding && Input.GetKeyDown(KeyCode.Z))
        {
            isThrowing = true;
            isHolding = false;
            if (Input.GetAxis("Horizontal") < 0)
            {
                targetPos = new Vector3(transform.position.x - 6.0f, transform.position.y, 0);
            }
            else
            {
                targetPos = new Vector3(transform.position.x + 6.0f, transform.position.y, 0);
            }
        } 

        if (isThrowing)
        {
            ThrowBoomerang();
        }

        if (isHolding)
        {
            Vector3 offSetPos = new Vector3(playerObj.transform.position.x + 0.5f, playerObj.transform.position.y - 0.5f, 0);
            transform.position = offSetPos;
            isRotating = false;
        }

        if (!isHolding && !isThrowing && !isDropped)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerObj.transform.position, movingSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, playerObj.transform.position) <= 0.01f)
            {
                isRotating = false;
                isHolding = true;
            }
        }

        if(Vector2.Distance(transform.position, targetPos) <= 0.01f)
        {
            isThrowing = false;
        }
    }

    private void ThrowBoomerang()
    {
        isRotating = true;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, movingSpeed * Time.deltaTime);
    }

    private void RotateWeapon()
    {
        if(isRotating)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isHolding = true;
            isDropped = false;
        }
        if (other.gameObject.tag.Equals("monsterShooter"))
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag.Equals("Walls") || other.gameObject.tag.Equals("Spikes"))
        {
            isThrowing = false;
            isDropped = true;
            isRotating = false;
        }
    }
}
