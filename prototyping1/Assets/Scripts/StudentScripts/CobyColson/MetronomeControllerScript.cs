using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeControllerScript : MonoBehaviour
{
    public GameObject barPrefab;
    public GameObject statusTextPrefab;
    public GameObject metronomeMarkerPrefab;
    private GameObject metronomeMarker;
    private GameObject canvas;
    private List<GameObject> barsList;
    private MetronomeStatusScript status;
    private GameHandler handler;
    private GameObject player;
    private MetronomePlayerControllerScript pMove;
    private Animator pAnim;
    private AudioSource[] audioSources;
    public int bars;
    private int canvasHalfWidth = 640;
    private int canvasHalfHeight = 360;
    public float perfectThreshold;
    public float goodThreshold;
    private bool actionThisBeat = false;
    private bool actionStartedThisRound = false;
    private float actionCooldownTime = 0.0f;
    void Awake()
    {
        canvas = GameObject.Find("Canvas");

        // Spawn objects
        GameObject statusText = Instantiate(statusTextPrefab) as GameObject;
        statusText.transform.SetParent(canvas.transform);
        statusText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.0f, -210.0f, -3.0f);

        metronomeMarker = Instantiate(metronomeMarkerPrefab) as GameObject;
        metronomeMarker.transform.SetParent(canvas.transform);
        metronomeMarker.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.0f, -283.0f, 0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        player = GameObject.Find("Player");
        if (player.GetComponent<PlayerMove>() != null)
        {
            Destroy(player.GetComponent<PlayerMove>());
        }
        pMove = player.AddComponent<MetronomePlayerControllerScript>();
        pAnim = player.GetComponentInChildren<Animator>();

        //metronomeMarker = GameObject.Find("MetronomeMarker");
        barsList = new List<GameObject>();
        status = GetComponent<MetronomeStatusScript>();
        audioSources = GetComponents<AudioSource>();
        if (bars % 2 != 0)
        {
            // Number of bars must be even
            ++bars;
        }
        for (int i = 0; i < bars; ++i)
        {
            GameObject bar = Instantiate(barPrefab) as GameObject;
            barsList.Add(bar);
            bar.transform.SetParent(metronomeMarker.transform, false);
        }
        Debug.Log(barsList.Count);
        ResetBarPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale >= 1.0f)
        {
            actionCooldownTime -= Time.deltaTime;
            EnableUI();
            FadeInMusic();

            bool valid = false;
            MetronomeStatusScript.StatusPreset preset = MetronomeStatusScript.presets[2];
            for (int i = 0; i < bars; ++i)
            {
                if (Mathf.Abs(barsList[i].GetComponent<RectTransform>().anchoredPosition.x) < goodThreshold)
                {
                    valid = true;
                    preset = MetronomeStatusScript.presets[1];
                    if (Mathf.Abs(barsList[i].GetComponent<RectTransform>().anchoredPosition.x) < perfectThreshold)
                    {
                        preset = MetronomeStatusScript.presets[0];
                    }
                }
            }
            
            Vector3 move = GetPlayerInput();
            if (move != Vector3.zero && actionCooldownTime <= 0.0f)
            {
                if (valid)
                {
                    if (preset.text == MetronomeStatusScript.presets[0].text)
                    {
                        audioSources[0].pitch = 1.0f;
                        if (!actionStartedThisRound) // Only start the game on a perfect hit
                        {
                            actionStartedThisRound = true;
                            audioSources[3].Play();
                        }
                    }
                    else if (preset.text == MetronomeStatusScript.presets[1].text)
                    {
                        audioSources[0].pitch = 0.8f;
                        if (!actionStartedThisRound) // Delay game start for perfect hit
                        {
                            preset = MetronomeStatusScript.presets[4];
                        }
                    }
                    if (actionStartedThisRound)
                    {
                        pMove.MovePlayer(move);
                        audioSources[0].Play(0);
                    }
                }
                else
                {
                    audioSources[2].Play(0);
                    pAnim.SetTrigger("Hurt");
                    handler.TakeDamage(5);
                }
                status.SetTextToPreset(preset);
                actionCooldownTime = 0.33f;
                pAnim.SetBool("Walk", true);
            }
            else
            {
                pAnim.SetBool("Walk", false);
            }
        }
        else
        {
            DisableUI();
            FadeOutMusic();
        }
    }

    public void ResetActions()
    {
        actionThisBeat = false;
    }

    private void ResetBarPositions()
    {
        float spacing = 2 * canvasHalfWidth / bars;
        float xPos = -canvasHalfWidth;
        for (int i = 0; i < barsList.Count; ++i)
        {
            if (i == (barsList.Count / 2))
            {
                xPos += spacing;
            }
            
            barsList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0.0f);
            xPos += spacing;
        }
    }

    private void EnableUI()
    {
        if (!metronomeMarker.activeSelf)
        {
            metronomeMarker.SetActive(true);
            for (int i = 0; i < barsList.Count; ++i)
            {
                barsList[i].SetActive(true);
            }
            ResetBarPositions();
        }
    }

    private void DisableUI()
    {
        if (metronomeMarker.activeSelf)
        {
            metronomeMarker.SetActive(false);
            for (int i = 0; i < barsList.Count; ++i)
            {
                barsList[i].SetActive(false);
            }
        }
    }

    private Vector3 GetPlayerInput()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            move = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            move = Vector3.left;

            Vector3 newScale = player.transform.localScale;
            if (newScale.x > 0)
            {
                newScale.x = -newScale.x;
            }
            player.transform.localScale = newScale;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            move = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            move = Vector3.right;

            Vector3 newScale = player.transform.localScale;
            if (newScale.x < 0)
            {
                newScale.x = -newScale.x;
            }
            player.transform.localScale = newScale;
        }
        return move;
    }

    private void FadeInMusic()
    {
        if (!audioSources[3].isPlaying && actionStartedThisRound)
        {
            audioSources[3].Play();
        }
        if (audioSources[3].volume < 1.0f)
        {
            audioSources[3].volume += Time.unscaledDeltaTime;
        }
    }

    private void FadeOutMusic()
    {
        if (audioSources[3].volume > 0.0f)
        {
            audioSources[3].volume -= Time.unscaledDeltaTime;
        }
        else if (audioSources[3].isPlaying)
        {
            audioSources[3].Stop();
        }
    }
}
