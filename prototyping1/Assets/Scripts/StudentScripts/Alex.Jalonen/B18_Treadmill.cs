using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class B18_Treadmill : MonoBehaviour
{
    public enum Speed
    {
        Slow,
        Medium,
        Fast
    }

    public bool setSpeed = false;
    public float slowSpeed_;
    public float medSpeed_;
    public float fastSpeed_;

    private static float slowSpeed = 20f;
    private static float mediumSpeed = 30f;
    private static float fastSpeed = 50f;

    public Speed speed;

    private float tileSpeed = 0f;

    private static Dictionary<int, Vector2> moveList = new Dictionary<int, Vector2>();
    private static Dictionary<int, bool> updateList = new Dictionary<int, bool>();

    private void Start()
    {
        switch(speed)
        {
            case Speed.Slow: tileSpeed = slowSpeed; break;
            case Speed.Medium: tileSpeed = mediumSpeed; break;
            case Speed.Fast: tileSpeed = fastSpeed; break;
        }

        slowSpeed_ = slowSpeed; medSpeed_ = mediumSpeed; fastSpeed_ = fastSpeed;


        gameObject.GetComponent<Animator>().speed =  (tileSpeed / mediumSpeed) * 4.2f;
    }

    private void Update()
    {
        if(setSpeed)
        {
            slowSpeed = slowSpeed_; mediumSpeed = medSpeed_; fastSpeed = fastSpeed_;
        }
        else
        {
            slowSpeed_ = slowSpeed; medSpeed_ = mediumSpeed; fastSpeed_ = fastSpeed;
        }
    }

    private void FixedUpdate()
    {
        foreach(var key in new List<int>(updateList.Keys))
        {
            updateList[key] = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherRB = collision.gameObject.GetComponent<Rigidbody2D>();

        if (otherRB != null)
        {
            var otherRBID = otherRB.GetInstanceID();

            Vector2 direction;

            if (moveList.ContainsKey(otherRBID))
            {
                var vec = moveList[otherRBID];
                vec += (Vector2)gameObject.transform.up;
                moveList[otherRBID] = vec;
            }
            else
            {
                var vec = (Vector2)gameObject.transform.up;
                moveList.Add(otherRBID, vec);
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var otherRB = collision.gameObject.GetComponent<Rigidbody2D>();

        if(otherRB != null)
        {
            var otherRBID = otherRB.GetInstanceID();

            if (updateList.ContainsKey(otherRBID) && updateList[otherRBID] == true) return;

            Vector2 direction;

            if(moveList.ContainsKey(otherRBID))
            {
                var vec = moveList[otherRBID];
                direction = vec.normalized;
            }
            else
            {
                var vec = (Vector2)gameObject.transform.up;
                moveList.Add(otherRBID, vec);
                direction = vec.normalized;
            }

            otherRB.velocity += direction * tileSpeed * Time.deltaTime;

            if(updateList.ContainsKey(otherRBID))
            {
                updateList[otherRBID] = true;
            }
            else
            {
                updateList.Add(otherRBID, true);
            }
            
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var otherRB = collision.gameObject.GetComponent<Rigidbody2D>();

        if (otherRB != null)
        {
            var otherRBID = otherRB.GetInstanceID();

            Vector2 direction;

            if (moveList.ContainsKey(otherRBID))
            {
                var vec = moveList[otherRBID];
                vec -= (Vector2)gameObject.transform.up;
                moveList[otherRBID] = vec;
            }
            else
            {
                var vec = (Vector2)gameObject.transform.up;
                moveList.Add(otherRBID, Vector2.zero);
            }

        }
    }

}
