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
        float spacing = 2 * canvasHalfWidth / bars;
        float xPos = -canvasHalfWidth;
        for (int i = 0; i < bars; ++i)
        {
            if (i == (bars / 2))
            {
                xPos += spacing;
            }
            GameObject bar = Instantiate(barPrefab) as GameObject;
            barsList.Add(bar);
            bar.transform.SetParent(metronomeMarker.transform, false);
            bar.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0.0f);
            xPos += spacing;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool valid = false;
        Vector3 move = Vector3.zero;
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
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            move = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            move = Vector3.left;

            Vector3 newScale = player.transform.localScale;
			newScale.x = -1.0f;
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
			newScale.x = 1.0f;
			player.transform.localScale = newScale;
        }
        if (move != Vector3.zero && !actionThisBeat)
        {
            if (valid)
            {
                pMove.MovePlayer(move);
                if (preset.text == MetronomeStatusScript.presets[0].text)
                {
                    MetronomeBarScript.barSpeed += 0.5f * Mathf.Sqrt(MetronomeBarScript.barSpeed);
                    audioSources[0].pitch = 1.0f;
                }
                else if (preset.text == MetronomeStatusScript.presets[1].text)
                {
                    audioSources[0].pitch = 0.8f;
                }
                audioSources[0].Play(0);
            }
            else
            {
                audioSources[2].Play(0);
                MetronomeBarScript.barSpeed = MetronomeBarScript.initialBarSpeed;
                pAnim.SetTrigger("Hurt");
                handler.TakeDamage(5);
            }
            status.SetTextToPreset(preset);
            actionThisBeat = true;
            pAnim.SetBool("Walk", true);
        }
        else
        {
            pAnim.SetBool("Walk", false);
        }
    }

    public void ResetActions()
    {
        actionThisBeat = false;
    }
}
