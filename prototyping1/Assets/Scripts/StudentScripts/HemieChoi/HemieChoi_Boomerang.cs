using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HemieChoi_Boomerang : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movingSpeed;
    private bool isRotating;
    private Vector3 targetPos;
    private bool isThrowing;
    [SerializeField] private bool isDropped;
    [SerializeField] private bool isHolding;
    private GameObject playerObj;
    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 1000.0f;
        movingSpeed = 8.0f;
        playerObj = GameObject.Find("Player");
        isHolding = true;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        RotateWeapon();

        if (isHolding && Input.GetMouseButtonDown(0))
        {
            isThrowing = true;
            isHolding = false;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos = new Vector3(mousePos.x, mousePos.y, 0);
            //targetPos.x = Mathf.Clamp(targetPos.x, targetPos.x >= 0 ? targetPos.x - (targetPos.x - 3.0f) : targetPos.x + (targetPos.x - 3.0f))
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

        // make it return
        if ((!isHolding && !isThrowing && !isDropped))
        {
            transform.position = Vector2.MoveTowards(transform.position, playerObj.transform.position, movingSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, playerObj.transform.position) <= 0.01f)
            {
                isRotating = false;
                isHolding = true;
            }
        }

        if(Vector2.Distance(transform.position, targetPos) <= 0.01f || timer > 0.5f)
        {
            isThrowing = false;
        }

        if (!isThrowing)
        {
            timer = 0;
        }
    }

    private void ThrowBoomerang()
    {
        isRotating = true;
        timer += Time.deltaTime;
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
        if (other.gameObject.tag.Equals("monsterShooter") && !isHolding && !isDropped)
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
