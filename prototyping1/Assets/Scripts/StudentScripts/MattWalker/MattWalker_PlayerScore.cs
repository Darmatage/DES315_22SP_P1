using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MattWalker_PlayerScore : MonoBehaviour
{

    private int Score;
    private Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        ScoreText = transform.GetChild(0).gameObject.GetComponent<Text>();
        ScoreText.text = "Score: " + Score;
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = "Score: " + Score;
    }

    public void IncreaseScore(int amount)
	{
        Score += amount;
	}
}
