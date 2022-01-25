using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public GameObject ContainedObject;
    public GameObject PromptObjectUI;
    public GameObject ExplosionPrefab;
    public Sprite ObjectSprite;

    private GameObject Player;
    private Transform PlayerTrans;
    private bool IsColliding;

    // UI prompt support
    private bool IsDisplayingPrompt;
    private GameObject UIPromptInstance;
    private float PromptDelay = 1.0f;
    private float PromptTimer;

    // Destruction timer support
    private float DestructionDelay = 0.25f;
    private float DestructionTimer;
    private bool DestructionImminent;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTrans = Player.GetComponent<Transform>();

        IsColliding = false;
        IsDisplayingPrompt = false;

        DestructionTimer = 0.0f;
        DestructionImminent = false;

        PromptTimer = 0.0f;

        // Attatch the sprite to the child object
        if (ObjectSprite != null)
		{
            SpriteRenderer sr = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = ObjectSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsColliding)
		{
            // Only display the prompt to destroy the object if
            // the player is facing the object (or was moving 
            // towards the object with the last player input)
            if (ShouldDisplayDestroyPrompt())
			{
                if (!IsDisplayingPrompt && !DestructionImminent)
				{
                    // The prompt should be displaying
                    UIPromptInstance = Instantiate(PromptObjectUI, PlayerTrans, false);
                    IsDisplayingPrompt = true;
                }
			}
            else if (IsDisplayingPrompt)
			{
                RemovePrompt();
            }

            // While colliding with the object, the player should be able to destroy it
            if (Input.GetKey(KeyCode.Space)) // Spacebar destroys the object
            {
                DestructionImminent = true;

                if (IsDisplayingPrompt)
                {
                    RemovePrompt();
                }
            }
        }
         
        if (DestructionImminent) // start the destruction timer
            DestroyAfterDelay();
    }

    void DestroyAfterDelay()
	{
        DestructionTimer += Time.deltaTime;

        if (DestructionTimer >= DestructionDelay)
		{
            if (ContainedObject != null)
                GameObject.Instantiate(ContainedObject, transform.position, Quaternion.identity);

            if (ExplosionPrefab != null)
                GameObject.Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    bool ShouldDisplayDestroyPrompt()
	{
        bool shouldDisplayPrompt = false;

        PromptTimer += Time.deltaTime;
        if (PromptTimer >= PromptDelay)
            shouldDisplayPrompt = true;
        
    //    // The player must also push at the object to be able to destroy it
    //    float xDiff = PlayerTrans.position.x - transform.position.x;
    //    float yDiff = PlayerTrans.position.y - transform.position.y;
    //    if (Mathf.Abs(xDiff) < Mathf.Abs(yDiff)) // Player should aim up or down
    //    {
    //        if (yDiff < 0) // DO is above player, so player should point up
    //        {
    //            if (Input.GetKey(KeyCode.W))
    //                shouldDisplayPrompt = true;
    //            else if (IsDisplayingPrompt)
				//{
    //                // As long as the player isn't actively moving to 
    //                // face away from the object, allow them to destroy it
    //                if (!Input.GetKey(KeyCode.S))
    //                    if (!Input.GetKey(KeyCode.D))
    //                        if (!Input.GetKey(KeyCode.A))
    //                            shouldDisplayPrompt = true;
    //            }
    //        }
    //        else // DO is below player, so player should point down
    //        {
    //            if (Input.GetKey(KeyCode.S))
    //                shouldDisplayPrompt = true;
    //            else if (IsDisplayingPrompt)
    //            {
    //                // As long as the player isn't actively moving to 
    //                // face away from the object, allow them to destroy it
    //                if (!Input.GetKey(KeyCode.W))
    //                    if (!Input.GetKey(KeyCode.D))
    //                        if (!Input.GetKey(KeyCode.A))
    //                            shouldDisplayPrompt = true;
    //            }
    //        }
    //    }
    //    else // Player should aim left or right
    //    {
    //        if (xDiff < 0) // DO is to the right of player, so player should point right
    //        {
    //            if (Input.GetKey(KeyCode.D))
    //                shouldDisplayPrompt = true;
    //            else if (IsDisplayingPrompt)
    //            {
    //                // As long as the player isn't actively moving to 
    //                // face away from the object, allow them to destroy it
    //                if (!Input.GetKey(KeyCode.S))
    //                    if (!Input.GetKey(KeyCode.W))
    //                        if (!Input.GetKey(KeyCode.A))
    //                            shouldDisplayPrompt = true;
    //            }
    //        }
    //        else // DO is to the left of player, so player should point left
    //        {
    //            if (Input.GetKey(KeyCode.A))
    //                shouldDisplayPrompt = true;
    //            else if (IsDisplayingPrompt)
    //            {
    //                // As long as the player isn't actively moving to 
    //                // face away from the object, allow them to destroy it
    //                if (!Input.GetKey(KeyCode.S))
    //                    if (!Input.GetKey(KeyCode.D))
    //                        if (!Input.GetKey(KeyCode.W))
    //                            shouldDisplayPrompt = true;
    //            }
    //        }
    //    }

        return shouldDisplayPrompt;
	}

    void RemovePrompt()
	{
        if (UIPromptInstance != null)
            Destroy(UIPromptInstance);

        IsDisplayingPrompt = false;
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsColliding = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsColliding = false;

            if (IsDisplayingPrompt)
            {
                RemovePrompt();
            }
        }
    }

}
