using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KobeDennis_PlayerInputScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 fireDirection;
    [SerializeField]
    private Vector3 lastFireDirection;

    public GameObject Lavaball_prefab;
    // Start is called before the first frame update
    void Start()
    {
        fireDirection = Vector3.zero;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var lavaBall = Instantiate(Lavaball_prefab, gameObject.transform.position + lastFireDirection, Quaternion.identity) as GameObject;
            lavaBall.GetComponent<KobeDennis_LavaBallScript>().SetDirection(lastFireDirection);

        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        fireDirection = Vector3.zero;
        fireDirection.x = Input.GetAxisRaw("Horizontal");
        fireDirection.y = Input.GetAxisRaw("Vertical");
        if(fireDirection != Vector3.zero)
        lastFireDirection = fireDirection;


        if(fireDirection.x > 0 )
        {
            Debug.Log("Right");

        }
        else if (fireDirection.x < 0)
        {
            Debug.Log("Left");

        }
        
    }
}
