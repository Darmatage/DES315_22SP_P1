using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grate : MonoBehaviour
{

    public SpriteRenderer spr_closed;
    public SpriteRenderer spr_open;

    public bool open = false;

    // Start is called before the first frame update
    void Start()
    {
        open = !open;
        Toggle();
    }

    public void Open()
    {
        open = true;
        spr_open.gameObject.SetActive(true);
        spr_closed.gameObject.SetActive(false);
        gameObject.layer = 22;
    }

    public void Close()
    {
        open = false;
        spr_open.gameObject.SetActive(false);
        spr_closed.gameObject.SetActive(true);
        gameObject.layer = 0;
    }

    public void Toggle()
    {
        if (open)
            Close();
        else
            Open(); 
    }
}
