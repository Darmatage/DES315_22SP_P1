using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindControlHelmet : MonoBehaviour
{
    // Start is called before the first frame update

    List<Transform> enemies = new List<Transform>();
    CameraFollow camera = null;
    PlayerMove player = null;
    MonoBehaviour enemyComp = null;
    bool onPlayer = true;
    float playerMoveSpeed = 0;
    bool toDisable = false;
    bool isKinematic = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemies.Add(collision.transform);
        }
        else if (!onPlayer && collision.CompareTag("Switch"))
        {
            toDisable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        enemies.Remove(other.transform);
    }

    void Start()
    {
        transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        playerMoveSpeed = player.speed;
    }

    Transform GetClosestEnemy()
    {
        if (enemies.Count == 0 || (enemies.Count == 1 && enemies[0] == transform.parent))
        {
            return null;
        }
        float mindist = float.MaxValue;
        Transform enemy = null;
        foreach(Transform e in enemies)
        {
            if (e !=null && e != transform.parent && Vector2.Distance(e.position, transform.position) < mindist)
            {
                mindist = Vector2.Distance(e.position, transform.position);
                enemy = e;
            }
        }
        return enemy;
    }
    // Update is called once per frame
    void Update()
    {
       
        if(Input.GetKeyDown(KeyCode.Space) && enemies.Count > 0)
        {
            Transform enemy = GetClosestEnemy();
            if (enemy != null)
            {
                if (enemyComp != null)
                {
                    enemyComp.enabled = true;

                }
                if (enemy.GetComponent<MonsterMoveHit>() != null)
                {
                    enemyComp = enemy.GetComponent<MonsterMoveHit>();
                }
                else if (enemy.GetComponent<MonsterShootMove>() != null)
                {
                    enemyComp = enemy.GetComponent<MonsterShootMove>();
                }
                enemyComp.enabled = false;
                isKinematic = enemyComp.GetComponent<Rigidbody2D>().isKinematic;
                enemyComp.GetComponent<Rigidbody2D>().isKinematic = false;
                transform.parent = enemy;
                transform.localPosition = new Vector3(0, 0, 0);
                camera.playerObj = enemy.gameObject;
                onPlayer = false;  
                player.speed = 0;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            enemyComp.enabled = true;            transform.parent = player.transform;
            transform.localPosition = new Vector3(0, 0, 0);
            camera.playerObj = player.gameObject;
            onPlayer = true;
            player.speed = playerMoveSpeed;
            enemyComp.GetComponent<Rigidbody2D>().isKinematic = isKinematic;
        }

        
    }

    protected virtual void FixedUpdate()
    {
        if (transform.parent != player.transform)
        {
            
                
                Vector3 change = Vector3.zero;
                change.x = Input.GetAxisRaw("Horizontal");
                change.y = Input.GetAxisRaw("Vertical");

                transform.parent.GetComponent<Rigidbody2D>().MovePosition(transform.position + change * playerMoveSpeed * Time.deltaTime);


        }
        //else playerDie(); //run this function from the GameHandler instead
    }

    private void LateUpdate()
    {
        if (toDisable)
        {
            toDisable = false;
            enemyComp.enabled = true;
            enemyComp.transform.tag = "Untagged";
            transform.parent = player.transform;
            transform.localPosition = new Vector3(0, 0, 0);
            camera.playerObj = player.gameObject;
            onPlayer = true;
            enemyComp.GetComponent<Rigidbody2D>().isKinematic = isKinematic;
            player.speed = playerMoveSpeed;
            }
    }

}
