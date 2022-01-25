using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacobBurkeSummonClone : MonoBehaviour
{
    [SerializeField]
    GameObject playerClone;

    private GameObject playerCloneInstance;
    private GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x - 0.5f, player.transform.position.y, player.transform.position.z);

        if (Input.GetKeyDown(KeyCode.E) && (playerCloneInstance == null) && playerClone)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Vector3 pos = player.transform.position;
            pos.x += 0.1f;
            playerCloneInstance = Instantiate(playerClone, pos, Quaternion.identity);
            playerCloneInstance.GetComponent<JacobBurke_Cloning>().shadowSpawner = gameObject;
        }
    }
}
