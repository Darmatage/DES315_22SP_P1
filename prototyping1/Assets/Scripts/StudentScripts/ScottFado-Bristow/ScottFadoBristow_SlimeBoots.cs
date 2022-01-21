using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScottFadoBristow_SlimeBoots : MonoBehaviour
{

    public GameObject AttatchSlime;
    public Vector2 BootsOffset;
    public Vector2 SlimeOffset;
    private List<GameObject> slimes;

    // Start is called before the first frame update
    void Start()
    {
        slimes = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void Attatch()
    {
        //Create the new child and attatch it to this object
        GameObject newSlime = Instantiate(AttatchSlime, transform, false);
        float x = BootsOffset.x + Random.Range(-SlimeOffset.x, SlimeOffset.x);
        float y = BootsOffset.y + Random.Range(-SlimeOffset.y, SlimeOffset.y);
        newSlime.transform.localPosition = new Vector3(x, y, newSlime.transform.position.z);
        slimes.Add(newSlime);

    }
}
