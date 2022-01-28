using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JirakitJarusiripipat_GameManager : MonoBehaviour
{
    private int currentRound = 1;
    private int totalRound = 4;
    //public List<Transform[]> spawnLocation = new List<Transform[]>();

    [SerializeField]
    private Transform[] wave1SpawnPoint;
    [SerializeField]
    private Transform[] wave2SpawnPoint;
    [SerializeField]
    private Transform[] wave3SpawnPoint;
    [SerializeField]
    private Transform[] wave4SpawnPoint;
    public bool[] check;
    [SerializeField]
    private bool[] end;
    [SerializeField]
    private int[] totalSlimeNumber;
    [SerializeField]
    private int[] totalSkullNumber;
    [SerializeField]
    private int[] totalBossNumber;
    [SerializeField]
    private GameObject slime;
    [SerializeField]
    private GameObject skull;
    [SerializeField]
    private GameObject boss;
    public List<GameObject> allEnemy = new List<GameObject>();
    [SerializeField]
    private GameObject door;

    public Text waveText;
    //[SerializeField]
    //private Text waveParentText;
    [SerializeField]
    private JirakitJarusiripipat_SFX SFX;
    [SerializeField]
    private Text proceed;

    [SerializeField]
    Image instruction;

    [SerializeField]
    private bool gameStart = true;
    private bool gameEnd = false;
    private bool doorOpen = false;

    
    public bool hitActive = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameStart = true;
                if (SFX != null)
                    //SFX.BGM.Play();
                if (instruction != null)
                    instruction.gameObject.SetActive(false);
                if (proceed != null)
                    proceed.gameObject.SetActive(false);
            }
        }
        else
        {
            if(hitActive)
            {
                if (currentRound == 1)
                {
                    int count = 0;

                    for (int i = 0; i < allEnemy.Count; i++)
                    {
                        if (allEnemy[i] == null)
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
                        gameEnd = true;
                    }
                }
                if (gameEnd && !doorOpen)
                {
                    if (waveText != null)
                    {
                        waveText.gameObject.SetActive(false);
                    }
                    doorOpen = true;
                    door.GetComponent<Door>().DoorOpen();
                }
                //if (currentRound == 5)
                //{
                //    waveParentText.gameObject.SetActive(false);
                //    door.GetComponent<Door>().DoorOpen();
                //    currentRound++;
                //}
                if (currentRound == 1 && !check[0])
                {
                    int i = 0;
                    for (int j = 0; j < totalBossNumber[0]; j++)
                    {
                        allEnemy.Add(Instantiate(boss, wave1SpawnPoint[i].position, Quaternion.identity));
                        i++;
                    }
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
                }
                else if (currentRound == 2 && !check[1] && end[0])
                {
                    //Debug.Log("Total Slime = " + totalSlimeNumber[1]);

                    int i = 0;
                    for (int j = 0; j < totalBossNumber[1]; j++)
                    {
                        allEnemy.Add(Instantiate(boss, wave2SpawnPoint[i].position, Quaternion.identity));
                        i++;
                    }
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
                    for (int j = 0; j < totalBossNumber[2]; j++)
                    {
                        allEnemy.Add(Instantiate(boss, wave3SpawnPoint[i].position, Quaternion.identity));
                        i++;
                    }
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
                else if (currentRound == totalRound && !check[3] && end[2])
                {
                    int i = 0;
                    for (int j = 0; j < totalBossNumber[3]; j++)
                    {
                        allEnemy.Add(Instantiate(boss, wave4SpawnPoint[i].position, Quaternion.identity));
                        i++;
                    }
                    for (int j = 0; j < totalSlimeNumber[3]; j++)
                    {
                        allEnemy.Add(Instantiate(slime, wave4SpawnPoint[i].position, Quaternion.identity));
                        i++;
                    }
                    for (int k = 0; k < totalSkullNumber[3]; k++)
                    {
                        allEnemy.Add(Instantiate(skull, wave4SpawnPoint[i].position, Quaternion.identity));
                        i++;
                    }
                    check[3] = true;
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    GameObject[] obj = GameObject.FindGameObjectsWithTag("monsterShooter");
                    foreach (var item in obj)
                    {
                        Destroy(item);
                    }

                }
                if (waveText != null)
                {
                    //roundNumberText.text = currentRound.ToString();
                    waveText.text = "Wave " + currentRound.ToString() + " / " + totalRound.ToString();
                }
            }
        }
            
    }

}
