using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Triggers a quick time event on collision with the given tag (default Player)
public class B05_QTETrigger : MonoBehaviour
{
    //When colliding with an object with this tag, we will trigger the QTE event
    public string TargetTag = "Player";

    /*
     * This is defaulted to null in case you want to implement your own system
     * If you want to use mine, set this to the B05_QTEObject in the BarBenzvi folder in StudentPrefabs
    */
    public GameObject QuickTimeEventPrefab = null;
    //This is the parent that will be given to the QTE prefab when spawned. Keep null if you don't want a parent
    public GameObject SpawnParent = null;

    /*
     * Changes global timescale to this value when the QTE is triggered
     * I suggest having this set to 1.0f if your QTE object does not reset it on finishing
     * I would also suggest having your QTE object(s) be timescale independent if not using 1.0f
     */
    public float QTETimescale = 0.1f;

    //Set this to true if you want this specific actor to be destroyed when the player succeeds at the QTE triggered by this actor
    public bool DestroyOnTriggeredQTESuccess = true;
    public bool GetsHitOnTriggeredQTESuccess = false;

    public float DestroyOnSuccessDelay = 0.5f;

    bool triggeredQTE = false;
    bool destroying = false;

    private Renderer rend;
    private Animator anim;

    private void OnEnable()
    {
        B05_EventManager.OnQTESuccess += OnQTESuccess;
        B05_EventManager.OnQTEFailure += OnQTEFailure;
    }

    private void OnDisable()
    {
        B05_EventManager.OnQTESuccess -= OnQTESuccess;
        B05_EventManager.OnQTEFailure -= OnQTEFailure;
    }

    private void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TargetTag) && !destroying)
        {
            triggeredQTE = true;
            //Spawn QTE object
            if (QuickTimeEventPrefab != null)
            {
                GameObject.Instantiate(QuickTimeEventPrefab, SpawnParent.transform);
            }
            Time.timeScale = QTETimescale;
            B05_EventManager.CallQTEStarted();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TargetTag) && !destroying)
        {
            triggeredQTE = true;
            //Spawn QTE object
            if(QuickTimeEventPrefab != null)
            {
                GameObject.Instantiate(QuickTimeEventPrefab, SpawnParent.transform);
            }
            Time.timeScale = QTETimescale;
            B05_EventManager.CallQTEStarted();
        }
    }

    //An event is sent by the B05 QTE object when the QTE is succeeded 
    void OnQTESuccess()
    {
        if (triggeredQTE)
        {
            if(DestroyOnTriggeredQTESuccess)
            {
                destroying = true;
                StartCoroutine(DestroyObjectAfterDelay(DestroyOnSuccessDelay));
            }
            else if(GetsHitOnTriggeredQTESuccess)
            {
                GetComponent<B05_EnemyMove>().HitEnemy();
            }

            triggeredQTE = false;
        }
    }

    //An event is sent by the B05 QTE object when the QTE is failed 
    void OnQTEFailure()
    {
        //Currently just resets this flag so that multiple enemies don't die at once when succeeding
        //a QTE after failing one on a different enemy
        triggeredQTE = false;
    }

    IEnumerator DestroyObjectAfterDelay(float delay)
    {
        if(anim && rend)
        {
            anim.SetTrigger("Hurt");
            // color values are R, G, B, and alpha, each divided by 100
            rend.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);
        }

        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
