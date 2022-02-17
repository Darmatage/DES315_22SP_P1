using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JasonHunt_BlockScript : MonoBehaviour
{
	[SerializeField]
    private GameObject Hammer; 

	public GameObject highlight;

    void Start(){
        Hammer = GameObject.Find("JasonHunt_Hammer");
        //highlight = GameObject.Find("JasonHunt_Highlight");
        highlight.SetActive(false);
    }


    public void explode(){
        GameObject effect = Instantiate(Hammer);
        effect.transform.position = transform.position;
        effect.GetComponent<JasonHunt_HammerScript>().SelfDestruct = true;
        effect.GetComponent<ParticleSystem>().Play();
    }

    public void ToggleHighlight(bool isOn){
        highlight.SetActive(isOn);
    }

}
