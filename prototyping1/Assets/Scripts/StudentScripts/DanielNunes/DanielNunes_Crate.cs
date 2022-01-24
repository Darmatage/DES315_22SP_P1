using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielNunes_Crate : MonoBehaviour
{
    [SerializeField]
    private Sprite shatter;
    [SerializeField]
    private GameObject particles;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DN_Cannonball"))
        {
            GetComponent<SpriteRenderer>().sprite = shatter;
            GetComponent<BoxCollider2D>().enabled = false;

            //create particles
            Instantiate(particles, transform);

            //reset all cannon contacts briefly (in the event a crate was shot right next to us)
            DanielNunes_Cannon cannonThatShotTheBallAtMe = collision.gameObject.transform.parent.GetComponent<DanielNunes_Cannon>();
            cannonThatShotTheBallAtMe.ResetContacts();
        }
    }
}
