using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class JirakitJarusiripipat_GameHandler : MonoBehaviour
{
	public GameObject healthText;
	public static int PlayerHealth = 100;
	public int PlayerHealthStart = 100;
	public GameObject playerObj;
	private bool isDead = false;
	private float deathTime = 10.0f;
	private float deathTimer = 0f;
	[SerializeField]
	public static bool GameisPaused = false;
	public GameObject pauseMenuUI;

	void Start()
	{

		if (GameObject.FindGameObjectWithTag("Player") != null)
		{
			playerObj = GameObject.FindGameObjectWithTag("Player");
		}

		Scene thisScene = SceneManager.GetActiveScene();
		if (thisScene.name == "MainMenu") { PlayerHealth = PlayerHealthStart; }

		UpdateHealth();
		pauseMenuUI.SetActive(false);
	}

	//pause menu
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameisPaused) { Resume(); }
			else { Pause(); }
		}
	}
	public void Heal(int healing)
	{
		PlayerHealth += healing;
		if (PlayerHealth >= PlayerHealthStart)
		{
			PlayerHealth = PlayerHealthStart;
		}
		UpdateHealth();
	}
	void Pause()
	{
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		GameisPaused = true;
	}

	public void Resume()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameisPaused = false;
	}

	public void Restart()
	{
		Time.timeScale = 1f;
		//restart the game:
		PlayerHealth = 100;
		SceneManager.LoadScene("MainMenu");
	}

	void FixedUpdate()
	{
		if (isDead == true)
		{
			deathTimer += 0.1f;
			if (deathTimer >= deathTime)
			{
				SceneManager.LoadScene("EndLose");
			}
		}
	}

	public void TakeDamage(int damage)
	{
		if(!playerObj.GetComponent<JirakitJarusiripipat_PlayerAction>().isUsingSkill)
        {
			PlayerHealth -= damage;
			if (PlayerHealth <= 0)
			{
				PlayerHealth = 0;
				playerObj.GetComponent<JirakitJarusiripipat_PlayerMove>().playerDie();
				isDead = true;
			}
			else
			{
				playerObj.GetComponent<JirakitJarusiripipat_PlayerMove>().playerHit();
			}
		}
		
		UpdateHealth();
	}
	
	public void UpdateHealth()
	{
		Text healthTextTemp = healthText.GetComponent<Text>();
		healthTextTemp.text = "HEALTH: " + PlayerHealth;
	}

	public void MainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void StartGame()
	{
		SceneManager.LoadScene("JirakitJarusiripipatScene");
	}
}
