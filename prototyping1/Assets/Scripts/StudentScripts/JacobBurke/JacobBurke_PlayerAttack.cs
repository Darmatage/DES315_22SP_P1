using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacobBurke_PlayerAttack : MonoBehaviour
{
    public int damage = 1;

    public GameObject attackCircle;
    private GameObject attackCircleInstance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!attackCircleInstance && attackCircle)
            {
                Vector3 pos = transform.position;

                if (gameObject.tag == "Player")
                    pos += gameObject.GetComponent<PlayerMove>().change;
                else
                    pos += gameObject.GetComponent<JacobBurke_Cloning>().change;
                
                attackCircleInstance = Instantiate(attackCircle, pos, Quaternion.identity);
            }
        }
    }
}
