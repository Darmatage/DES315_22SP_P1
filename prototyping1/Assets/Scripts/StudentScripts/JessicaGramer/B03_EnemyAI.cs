using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(B03_AStarPathFinding))]
public class B03_EnemyAI : MonoBehaviour
{
    enum AIState
    {
		//RETREAT,
		IDLE,
		CHASE,
		PATROL,
		FIND,

		// Bookeeping States
		NUM_STATES,
		INVALID = -1
	}

    public float Speed = 4f;
	public int Damage = 1;
	public int EnemyLives = 3;
	public float IdleTimer = 3.0f;

	private Transform target = null;
	private Vector3 prevTarget;
	private GameHandler gameHandler = null;
	private Renderer render = null;
	private Animator animator = null;
	private B03_AStarPathFinding pathFinding = null;
	private B03_TerrainAnaylsis terrainAnaylsis = null;

	private float idleTimer;
	private B03_Trigger FOVTrigger = null;
	private bool inFOV = false;

	[SerializeField] private AIState nextState = AIState.IDLE;
	private AIState currState = AIState.INVALID;
	[SerializeField] private Tilemap patrolRoutes = null;
	private Vector3Int patrolRoute;
	private Vector3Int prevRoute;
	private Vector3Int nextRoute;
	private bool isChanging = true;
	[SerializeField] float moveModifier;

	void Start()
	{
		animator = gameObject.GetComponentInChildren<Animator>();
		render = GetComponentInChildren<Renderer>();

		GameObject go_target = GameObject.FindGameObjectWithTag("Player");
		if (go_target != null)
		{
			target = go_target.GetComponent<Transform>();
		}
		GameObject go_game_handler = GameObject.FindWithTag("GameHandler");
		if (go_game_handler != null)
		{
			gameHandler = go_game_handler.GetComponent<GameHandler>();
		}
		GameObject go_terrain_anaylsis = GameObject.Find("TerrainAnaylsis");
		if (go_terrain_anaylsis != null)
		{
			terrainAnaylsis = go_terrain_anaylsis.GetComponent<B03_TerrainAnaylsis>();
		}

		pathFinding = GetComponent<B03_AStarPathFinding>();
		FOVTrigger = GetComponentInChildren<B03_Trigger>();

		Assert.IsNotNull(target, "There was no player found.");
		Assert.IsNotNull(gameHandler, "There was no handler found.");
		Assert.IsNotNull(terrainAnaylsis, "There was no terrain anaylsis found.");
		Assert.IsNotNull(FOVTrigger, "Make sure there is a child with a collder and a trigger compoenent.");
		Assert.IsNotNull(patrolRoutes, "Please set the patrol route for the AI Pathing");
		Assert.IsNotNull(pathFinding, "How do you not have an A* path finding compoent on this?!?");

		FOVTrigger.OnEnter = FOVOn;
		FOVTrigger.OnExit = FOVOff;
		prevTarget = target.position;

		patrolRoute = patrolRoutes.WorldToCell(transform.position);
		prevRoute = patrolRoute;

		moveModifier = Vector3.Distance(transform.position, patrolRoutes.CellToWorld(patrolRoute));
	}

	void Update()
	{
		//if (retreatTimer > 0.0f) retreatTimer -= Time.deltaTime;

		if (isChanging)
        {
			OnExit();

			currState = nextState;
			isChanging = false;

			OnInit();
        }

		OnUpdate();
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
			gameHandler.TakeDamage(Damage);
			nextState = AIState.FIND;

			//EnemyLives -= EnemyLives;
			//rend.material.color = new Color(2.4f, 0.9f, 0.9f, 0.5f);
			//Destroy(gameObject);
		}
	}

	void OnInit()
    {
        switch (currState)
        {
			case AIState.IDLE:
				idleTimer = IdleTimer;
				break;

			case AIState.CHASE:
				pathFinding.Begin = transform.position;
				pathFinding.Goal = target.position;

				pathFinding.NewRequest = true;
				break;

			case AIState.PATROL:
				nextRoute = patrolRoute;
				break;

			case AIState.FIND:
				pathFinding.Begin = transform.position;
				pathFinding.Goal = patrolRoutes.CellToWorld(patrolRoute);

				pathFinding.NewRequest = true;
				break;

			default:
				Debug.LogWarning("There seems to be no set AI State for enemy: " + name);
				break;
		}
    }

    void OnUpdate()
    {
		if (idleTimer > 0.0f) idleTimer -= Time.deltaTime;

		switch (currState)
		{
			case AIState.IDLE:
				if (idleTimer <= 0.0f && inFOV) SetState(AIState.CHASE);
				else if (idleTimer <= 0.0f) SetState(AIState.FIND);
				break;

			case AIState.CHASE:
				if (inFOV && target.position != prevTarget)
                {
					SetState(AIState.CHASE);
					prevTarget = target.position;
				}

				if(pathFinding.PrevResult == B03_AStarPathFinding.PathResult.COMPLETE) if (PathMove()) SetState(AIState.PATROL);
				break;

			case AIState.PATROL:
				if (inFOV) SetState(AIState.CHASE);

				// transform.position = Vector3.MoveTowards(transform.position, pathFinding.Path[pathFinding.Path.Count - 1], Speed * Time.deltaTime);
				Vector3Int curr_position = patrolRoutes.WorldToCell(transform.position);
				float distance = Vector3.Distance(curr_position, nextRoute);

				Vector3 position = patrolRoutes.CellToWorld(nextRoute);
				//position.Set(position.x - moveModifier, position.y - moveModifier, position.z);
				if (distance > 0.2) transform.position = Vector3.MoveTowards(transform.position, position, Speed * Time.deltaTime);
				else
                {
					// prevRoute = curr_position;
					Vector3Int n_position = new Vector3Int(curr_position.x, curr_position.y + 1, curr_position.z);
					Vector3Int s_position = new Vector3Int(curr_position.x, curr_position.y - 1, curr_position.z);
					Vector3Int e_position = new Vector3Int(curr_position.x + 1, curr_position.y, curr_position.z);
					Vector3Int w_position = new Vector3Int(curr_position.x - 1, curr_position.y, curr_position.z);
					// north
					if (n_position != prevRoute && patrolRoutes.HasTile(n_position))
					{
						nextRoute = n_position;
						prevRoute = curr_position;
					}

					// south
					else if (s_position != prevRoute && patrolRoutes.HasTile(s_position))
					{
						nextRoute = s_position;
						prevRoute = curr_position;
					}

					// east
					else if (e_position != prevRoute && patrolRoutes.HasTile(e_position))
					{
						nextRoute = e_position;
						prevRoute = curr_position;
					}

					// west
					else if (w_position != prevRoute && patrolRoutes.HasTile(w_position))
					{
						nextRoute = w_position;
						prevRoute = curr_position;
					}

					else prevRoute = Vector3Int.zero;
				}

				break;

			case AIState.FIND:
				if (inFOV) SetState(AIState.CHASE);
				if(pathFinding.PrevResult == B03_AStarPathFinding.PathResult.COMPLETE) 
					if (PathMove())
                    {
						prevRoute = patrolRoute;
						SetState(AIState.PATROL);
					}
				break;

			default:
				Debug.LogWarning("There seems to be no set AI State for enemy: " + name);
				break;
		}
	}

	void OnExit()
    {
		switch (currState)
		{
			case AIState.IDLE:
				break;

			case AIState.CHASE:
				break;

			case AIState.PATROL:
				break;

			case AIState.FIND:
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

	bool PathMove()
    {
		if (pathFinding.Path.Count != 0)
		{
			float distance = Vector3.Distance(transform.position, pathFinding.Path[pathFinding.Path.Count - 1]);

			if (distance <= 0.2)
			{
				pathFinding.Path.Remove(transform.position);
			}
			
			if(pathFinding.Path.Count != 0) 
				transform.position = Vector3.MoveTowards(transform.position, pathFinding.Path[pathFinding.Path.Count - 1], Speed * Time.deltaTime);
			return false;
		}

		else return true;
	}

	void SetState(AIState state)
    {
		nextState = state;
		isChanging = true;
    }

	void FOVOn()
    {
		Vector3 direction = Vector3.Normalize(target.position - transform.position);
		Vector2 v2_dir = new Vector2(direction.x, direction.y);
		Vector2 v2_pos = new Vector2(transform.position.x, transform.position.y);
		int layer_mask = LayerMask.GetMask("Default");
		float distance = Vector3.Distance(target.position, transform.position);

		var hits = Physics2D.Raycast(v2_pos, v2_dir, distance, layer_mask);

		if(hits.collider == null || hits.collider.gameObject.CompareTag("Player")) inFOV = true;
    }

	void FOVOff()
    {
		inFOV = false;
    }

    bool FindPlayer()
    {
        return false;
    }

    bool SeekPlayer()
    {
        return false;
    }
}
