using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JasonHunt_BlockScript : MonoBehaviour
{
    private GameObject Hammer;
    private GameObject highlight;
    // Start is called before the first frame update
    void Start()
    {
        Hammer = GameObject.Find("JasonHunt_Hammer");
        highlight = GameObject.Find("JasonHunt_Highlight");
        highlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void explode()
    {
        GameObject effect = Instantiate(Hammer);
        effect.transform.position = transform.position;
        effect.GetComponent<JasonHunt_HammerScript>().SelfDestruct = true;
        effect.GetComponent<ParticleSystem>().Play();
    }

    public void ToggleHighlight(bool isOn)
    {
        highlight.SetActive(isOn);
    }

}
