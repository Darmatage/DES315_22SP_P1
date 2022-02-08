using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScottFadoBristow_SlimeBoots : MonoBehaviour
{

    public GameObject AttatchSlime;
    public Vector2 BootsOffset;
    public Vector2 SlimeOffset;

    private GameHandler gameHandlerObj;

    public int MashAmount = 20;

    public GameObject MashToggle;

    private Stack<(GameObject, float)> slimes;
    private float wiggleTimer = 4;
    private bool wiggleDir = false;
    private int wiggleCount = 0;
    private GameObject player;

    public int DamageThreshold = 5;
    public float DamageRate = 1.0f;
    private float timer = 0;

    private float originalScaleX;

    // Start is called before the first frame update
    void Start()
    {
        slimes = new Stack<(GameObject, float)>();
        originalScaleX = MashToggle.transform.localScale.x;

        GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
        if (gameHandlerLocation != null)
        {
            gameHandlerObj = gameHandlerLocation.GetComponent<GameHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Alright!! Wiggle check
        //Check if player is moving left to right
        float input = Input.GetAxisRaw("Horizontal");

        Vector3 flipscale = MashToggle.transform.localScale;
        //Set the sign so it flips
        flipscale.x = originalScaleX * gameObject.transform.localScale.x / Mathf.Abs(gameObject.transform.localScale.x);
        MashToggle.transform.localScale = flipscale;
        

        if(Input.GetKeyDown(KeyCode.Space))
        {
            //SpriteRenderer sr = MashToggle.GetComponent<SpriteRenderer>();
            //sr.sprite = ToggleOn;
            wiggleCount++;
            foreach((GameObject, float) gf in slimes)
            {
                 gf.Item1.GetComponent<ScottFadoBristow_Wiggle>().StartWiggle();
            }
        }
        else
        {
            //SpriteRenderer sr = MashToggle.GetComponent<SpriteRenderer>();
            //sr.sprite = ToggleOff;
        }

        if(wiggleCount >= MashAmount)
        {
            Detach();
            wiggleCount = 0;
        }

        if (slimes.Count > DamageThreshold)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                gameHandlerObj.TakeDamage(1);
                timer = DamageRate;
            }
        }
    }



    public void Attatch(GameObject p, float speedDiff)
    {
        //Create the new child and attatch it to this object

        if(slimes.Count == 0)
        {
            MashToggle.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        GameObject newSlime = Instantiate(AttatchSlime, transform, false);
        float x = BootsOffset.x + Random.Range(-SlimeOffset.x, SlimeOffset.x);
        float y = BootsOffset.y + Random.Range(-SlimeOffset.y, SlimeOffset.y);
        newSlime.transform.localPosition = new Vector3(x, y, newSlime.transform.position.z);


        slimes.Push((newSlime, speedDiff));
        player = p;

    }


    public void Detach()
    {
        if (slimes.Count > 0)
        {
            (GameObject, float ) p = slimes.Pop();
            GameObject s = p.Item1;
            s.GetComponent<ScottFadoBristow_BootSlimeControl>().Kill();
            player.GetComponent<PlayerMove>().speed += p.Item2;
        }

        if(slimes.Count == 0)
        {
            
            MashToggle.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            
        }
    }

}
