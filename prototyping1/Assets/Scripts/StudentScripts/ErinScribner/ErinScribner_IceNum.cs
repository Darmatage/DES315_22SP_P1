using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErinScribner_IceNum : MonoBehaviour
{
    public Text IceText;
    private ErinScribner_PaintTile paint;
    private int currentNum = 0;
    private bool recharge = false;
    // Start is called before the first frame update
    void Start()
    {
       
        GameObject check = GameObject.Find("ErinScribner_PlaceBlock");
        paint = check.GetComponent<ErinScribner_PaintTile>();
        IceText.text = "Ice: " + currentNum + "/" + paint.numPower;


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && recharge == false)
        {
            currentNum++;
        }
        
        if(currentNum >= (paint.numPower + 1))
        {
            recharge = true;
        }
       
        if(recharge == true)
        {
            currentNum--;
        }

        if(currentNum == 0)
        {
            recharge = false;
        }


        IceText.text = "Ice: " + currentNum + "/" + paint.numPower;  
    }
}

/*public class PlayerScript : MonoBehaviour
{
    public float Health = 100.0f;
}

public class Accessor : MonoBehaviour
{
    void Start()
    {
        GameObject thePlayer = GameObject.Find("ThePlayer");
        PlayerScript playerScript = thePlayer.GetComponent<PlayerScript>();
        playerScript.Health -= 10.0f;
    }
}*/