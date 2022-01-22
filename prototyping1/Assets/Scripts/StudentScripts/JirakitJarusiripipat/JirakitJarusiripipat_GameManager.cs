using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JirakitJarusiripipat_GameManager : MonoBehaviour
{
    public int currentRound = 1;
    public int totalRound;
    //public List<Transform[]> spawnLocation = new List<Transform[]>();

    [SerializeField]
    private Transform[] wave1SpawnPoint;
    [SerializeField]
    private Transform[] wave2SpawnPoint;
    [SerializeField]
    private Transform[] wave3SpawnPoint;
    [SerializeField]
    private Transform[] wave4SpawnPoint;
    [SerializeField]
    private bool[] check;
    [SerializeField]
    private bool[] end;
    [SerializeField]
    private int[] totalSlimeNumber;
    [SerializeField]
    private int[] totalSkullNumber;

    [SerializeField]
    private GameObject slime;
    [SerializeField]
    private GameObject skull;
    [SerializeField]
    private GameObject boss;
    public List<GameObject> allEnemy = new List<GameObject>();
    [SerializeField]
    private GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentRound == 1)
        {
            int count = 0;

            for (int i = 0; i < allEnemy.Count; i++)
            {
                if(allEnemy[i] == null)
                {
                    count++;
                }

                
            }
            if (count == wave1SpawnPoint.Length)
            {
                allEnemy.Clear();
                currentRound++;
                end[0] = true;
            }
        }
        if (currentRound == 2)
        {
            int count = 0;

            for (int i = 0; i < allEnemy.Count; i++)
            {
                if (allEnemy[i] == null)
                {
                    count++;
                }
               
            }
            if (count == wave2SpawnPoint.Length)
            {
                allEnemy.Clear();
                currentRound++;
                end[1] = true;
            }
        }
        if (currentRound == 3)
        {
            int count = 0;

            for (int i = 0; i < allEnemy.Count; i++)
            {
                if (allEnemy[i] == null)
                {
                    count++;
                }
                
            }
            if (count == wave3SpawnPoint.Length)
            {
                allEnemy.Clear();
                currentRound++;
                end[2] = true;
            }
        }
        if (currentRound == 4)
        {
            int count = 0;

            for (int i = 0; i < allEnemy.Count; i++)
            {
                if (allEnemy[i] == null)
                {
                    count++;
                }

            }
            if (count == wave4SpawnPoint.Length)
            {
                allEnemy.Clear();
                currentRound++;
                end[3] = true;
            }
        }
        if(currentRound == 5)
        {
            door.GetComponent<Door>().DoorOpen();
            currentRound++;
        }
        if (currentRound == 1 && !check[0])
        {
            //for (int i = 0; i < wave1SpawnPoint.Length; i++)
            //{
            int i = 0;
            for (int j = 0; j < totalSlimeNumber[0]; j++)
            {
                allEnemy.Add(Instantiate(slime, wave1SpawnPoint[i].position, Quaternion.identity));
                i++;
            }
            for (int k = 0; k < totalSkullNumber[0]; k++)
            {
                allEnemy.Add(Instantiate(skull, wave1SpawnPoint[i].position, Quaternion.identity));
                i++;
            }
            check[0] = true;
            //}

        }
        else if (currentRound == 2 && !check[1] && end[0])
        {
            Debug.Log("Total Slime = " + totalSlimeNumber[1]);
            
            int i = 0;
            for (int j = 0; j < totalSlimeNumber[1]; j++)
            {
                allEnemy.Add(Instantiate(slime, wave2SpawnPoint[i].position, Quaternion.identity));
                i++;
            }

            for (int k = 0; k < totalSkullNumber[1]; k++)
            {
                allEnemy.Add(Instantiate(skull, wave2SpawnPoint[i].position, Quaternion.identity));
                i++;
            }
            check[1] = true;

        }
        else if (currentRound == 3 && !check[2] && end[1])
        {
            int i = 0;
            for (int j = 0; j < totalSlimeNumber[2]; j++)
            {
                allEnemy.Add(Instantiate(slime, wave3SpawnPoint[i].position, Quaternion.identity));
                i++;
            }
            for (int k = 0; k < totalSkullNumber[2]; k++)
            {
                allEnemy.Add(Instantiate(skull, wave3SpawnPoint[i].position, Quaternion.identity));
                i++;
            }
            check[2] = true;

        }
        else if(currentRound == totalRound && !check[3] && end[2])
        {
            for (int i = 0; i < 4; i++)
            {
                allEnemy.Add(Instantiate(boss, wave4SpawnPoint[i].position, Quaternion.identity));
            }
            check[3] = true;
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag("monsterShooter");
            foreach (var item in obj)
            {
                Destroy(item);
            }
            
        }
    }
}
