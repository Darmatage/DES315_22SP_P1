using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(B03_AStarPathFinding))]
public class B03_EnemyAI : MonoBehaviour
{
    enum AIState
    {
		//RETREAT,
		MOVE,
		FIND,
		TRACK,

		// Bookeeping States
		NUM_STATES,
		INVALID = -1
	}

    public float Speed = 4f;
	public int Damage = 1;
	public int EnemyLives = 3;
	public float RetreatTime = 3.0f;

	private Transform target = null;
	private GameHandler gameHandlerObj = null;
	private Renderer render = null;
	private Animator animator = null;
	private B03_AStarPathFinding pathFinding = null;

	private float retreatTimer;

	[SerializeField] private AIState nextState = AIState.FIND;
	private AIState currState = AIState.INVALID;
	private AIState prevState = AIState.INVALID;
	private List<Vector3> path = new List<Vector3>();

	void Start()
	{
		animator = gameObject.GetComponentInChildren<Animator>();
		render = GetComponentInChildren<Renderer>();

		if (GameObject.FindGameObjectWithTag("Player") != null)
		{
			target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		}
		GameObject gameHandlerLocation = GameObject.FindWithTag("GameHandler");
		if (gameHandlerLocation != null)
		{
			gameHandlerObj = gameHandlerLocation.GetComponent<GameHandler>();
		}

		pathFinding = GetComponent<B03_AStarPathFinding>();

		Assert.IsNotNull(target, "There was no player found.");
		Assert.IsNotNull(gameHandlerObj, "There was no handler found.");
		Assert.IsNotNull(pathFinding, "How do you not have an A* path finding compoent on this?!?");
	}

	void Update()
	{
		//if (retreatTimer > 0.0f) retreatTimer -= Time.deltaTime;

		if(nextState != currState)
        {
			onExit();

			prevState = currState;
			currState = nextState;

			onInit();
        }

		onUpdate();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "bullet")
		{
			StopCoroutine("GetHit");
			StartCoroutine("GetHit");
		}
		else if (collision.gameObject.tag == "Player")
		{
			//gameHandlerObj.TakeDamage(Damage);

			//EnemyLives -= EnemyLives;
			//rend.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);
			//Destroy(gameObject);
		}

		pathFinding.Goal = new Vector3(transform.position.x + Random.Range(-4, 4), transform.position.y, transform.position.z + Random.Range(-4, 4));
		nextState = AIState.FIND;
	}

	void onInit()
    {
        switch (currState)
        {
			case AIState.MOVE:
				break;

			case AIState.FIND:
				pathFinding.NewRequest = true;
				break;

			case AIState.TRACK:
				break;

			default:
				Debug.LogWarning("There seems to be no set AI State for enemy: " + name);
				break;
		}
    }

    void onUpdate()
    {
		switch (currState)
		{
			case AIState.MOVE:
				if (pathMove()) setState(AIState.FIND);
				break;

			case AIState.FIND:
				if (pathFinding.prevResult == B03_AStarPathFinding.PathResult.COMPLETE)
                {
					path = pathFinding.Path;

					pathFinding.Goal = target.position;
					setState(AIState.MOVE);
				}
				break;

			case AIState.TRACK:
				break;

			default:
				Debug.LogWarning("There seems to be no set AI State for enemy: " + name);
				break;
		}
	}

	void onExit()
    {
		switch (currState)
		{
			case AIState.MOVE:
				break;

			case AIState.FIND:
				break;

			case AIState.TRACK:
				break;

			default:
				Debug.LogWarning("There seems to be no set AI State for enemy: " + name);
				break;
		}
	}

	IEnumerator GetHit()
	{
		animator.SetTrigger("Hurt");
		EnemyLives -= 1;
		// color values are R, G, B, and alpha, each divided by 100
		render.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);
		if (EnemyLives < 1)
		{
			//gameHandlerObj.AddScore (1);

			Destroy(gameObject);
		}
		else yield return new WaitForSeconds(0.5f);
		render.material.color = Color.white;
	}

	bool pathMove()
    {
		if (path.Count != 0)
		{
			transform.position = Vector2.MoveTowards(transform.position, path[path.Count - 1], Speed * Time.deltaTime);
			if (transform.position == path[path.Count - 1]) path.Remove(transform.position);
			return false;
		}
		else return true;
	}

	void setState(AIState state)
    {
		nextState = state;
    }

	void fieldOfView()
    {

    }

    bool findPlayer()
    {
        return false;
    }

    bool seekPlayer()
    {
        return false;
    }
}
