using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class B03_AStarPathFinding : MonoBehaviour
{
    public enum PathResult // return values for path prossessing
    {   
        COMPLETE, // a path to the goal was found and has been built in request.path
        PROCESSING, //a path hasn't been found yet, should only be returned in single step mode until a path is found
        IMPOSSIBLE //a path from start to goal does not exist, do not add start position to path
    }

    public enum HeuristicType
    {
        OCTILE,
        CHEBYSHEV,
        MANHATTAN,
        EUCLIDEAN
    }

    public GameObject MainGrid = null;
    [Header("AI Properties")]
    [SerializeField] private List<Tilemap> NonCollidibles = new List<Tilemap>();
    [SerializeField] private bool canMoveDiagonal = true;
    [Header("Debug Coloring")]
    public Tilemap DebugVisusal = null;
    public bool DebugColoring = false;
    [SerializeField] private TileBase openList;
    [SerializeField] private TileBase closedList;
    [SerializeField] private TileBase goalTile;
    [Header("Single Step")]
    [SerializeField] bool SingleStep = false; // whether to perform only a single A* step
    [SerializeField] float StepTime = 1.0f; // the amount of seconds a single step takes
    [Header("Path Requesting")]
    public bool NewRequest = true; // recaluclate path?
    public Vector3 Goal = new Vector3Int(0, 0, 0);
    public Vector3 Begin = new Vector3Int(0, 0, 0);
    public List<Vector3> Path = new List<Vector3>(); // list of world positions
    public PathResult prevResult = PathResult.IMPOSSIBLE; // please no changy, needed for external code
    [Header("Misc.")]
    public HeuristicType Heuristic = HeuristicType.OCTILE;
    public float HeuristicWeight = 1.0f;

    private float timer = 0.0f;
    private const int north = 1 << 3, south = 1 << 2, east = 1 << 1, west = 1 << 0;

    private GridLayout gridLayout;
    private Vector3Int currStart;
    private Vector3Int currGoal;
    private List<B03_Node> openList_ = new List<B03_Node>();
    private List<B03_Node> closedList_ = new List<B03_Node>();

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(MainGrid, "Main Grid is not slected, find AStarPathFinding and set Main Grid to the Grid!");

        gridLayout = MainGrid.GetComponentInParent<GridLayout>();

        if(NonCollidibles.Count == 0) Debug.LogWarning("Are you sure you want the enemy to not avoid anything?");
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugColoring)
        {
            Assert.IsNotNull(DebugVisusal, "If you want debug coloring you need to set a debug visual tilemap.");

            Assert.IsNotNull(goalTile, "If you want debug coloring you need to sete the goal tile tile base.");
            Assert.IsNotNull(openList, "If you want debug coloring you need to sete the open list tile base.");
            Assert.IsNotNull(closedList, "If you want debug coloring you need to sete the closed list tile base.");
        }
        else if(DebugVisusal != null) DebugVisusal.ClearAllTiles();

        //transform.position = gridLayout.CellToWorld(Goal);
        if (NewRequest)
        {
            Path = new List<Vector3>();
            currStart = gridLayout.WorldToCell(Begin);
            currGoal = gridLayout.WorldToCell(Goal);
            prevResult = PathResult.PROCESSING;

            openList_ = new List<B03_Node>();
            closedList_ = new List<B03_Node>();

            B03_Node node = new B03_Node();
            node.Set(currStart, 0.0f);
            node.list_ = B03_Node.OnList.OpenList;
            openList_.Add(node);

            if (DebugColoring)
            {
                DebugVisusal.ClearAllTiles();

                DebugVisusal.SetTile(currGoal, goalTile);
                DebugVisusal.SetTile(node.pos_, openList);
            }
            NewRequest = false;
        }

        if(timer > 0.0f) timer -= Time.deltaTime;
        else if(prevResult == PathResult.PROCESSING) prevResult = ComputePath();
    }

    public PathResult ComputePath()
    {
        while(openList_.Count != 0)
        {
            // pop cheapest node
            B03_Node curr_node = openList_[0];
            for(int i = 0; i < openList_.Count; ++i)
                if (openList_[i].heuristicCost_ + openList_[i].givenCost_ <= 
                    curr_node.heuristicCost_ + curr_node.givenCost_) curr_node = openList_[i];

            // if node is goal node then return path found
            if(curr_node.pos_ == currGoal)
            {
                if (DebugColoring) DebugVisusal.SetTile(curr_node.pos_, closedList);

                while (curr_node != null)
                {
                    Path.Add(gridLayout.CellToWorld(curr_node.pos_));
                    curr_node = curr_node.prevNode_;
                }

                //Path.Reverse();
                return PathResult.COMPLETE;
            }

            // for all neighboring child nodes
            int walls = 0b1111;
            Vector3Int curr_pos = new Vector3Int(curr_node.pos_.x, curr_node.pos_.y, curr_node.pos_.z);

            // North
            curr_pos = new Vector3Int(curr_node.pos_.x, curr_node.pos_.y + 1, curr_node.pos_.z);
            if (isValidPosition(curr_pos))
            {
                childNode(curr_node, curr_pos);
                //std::cout << "North Square" << std::endl;
            }
            else walls = walls ^ north;
            // South
            curr_pos = new Vector3Int(curr_node.pos_.x, curr_node.pos_.y - 1, curr_node.pos_.z);
            if (isValidPosition(curr_pos))
            {
                childNode(curr_node, curr_pos);
                //std::cout << "South Square" << std::endl;
            }
            else walls = walls ^ south;
            // East
            curr_pos = new Vector3Int(curr_node.pos_.x + 1, curr_node.pos_.y, curr_node.pos_.z);
            if (isValidPosition(curr_pos))
            {
                childNode(curr_node, curr_pos);
                //std::cout << "East Square" << std::endl;
            }
            else walls = walls ^ east;
            // West
            curr_pos = new Vector3Int(curr_node.pos_.x - 1, curr_node.pos_.y, curr_node.pos_.z);
            if (isValidPosition(curr_pos))
            {
                childNode(curr_node, curr_pos);
                //std::cout << "East Square" << std::endl;
            }
            else walls = walls ^ west;

            // North East
            curr_pos = new Vector3Int(curr_node.pos_.x + 1, curr_node.pos_.y + 1, curr_node.pos_.z);
            if (isValidPosition(curr_pos) && canMoveDiagonal)
            {
                if ((walls & (north | east)) == (north | east))
                {
                    childNode(curr_node, curr_pos, true);
                    //std::cout << "North East Square" << std::endl;
                }
            }
            // South East
            curr_pos = new Vector3Int(curr_node.pos_.x + 1, curr_node.pos_.y - 1, curr_node.pos_.z);
            if (isValidPosition(curr_pos) && canMoveDiagonal)
            {
                if ((walls & (south | east)) == (south | east))
                {
                    childNode(curr_node, curr_pos, true);
                    //std::cout << "South East Square" << std::endl;
                }
            }
            // South West
            curr_pos = new Vector3Int(curr_node.pos_.x - 1, curr_node.pos_.y - 1, curr_node.pos_.z);
            if (isValidPosition(curr_pos) && canMoveDiagonal)
            {
                if ((walls & (south | west)) == (south | west))
                {
                    childNode(curr_node, curr_pos, true);
                    //std::cout << "South West Square" << std::endl;
                }
            }
            // North West
            curr_pos = new Vector3Int(curr_node.pos_.x - 1, curr_node.pos_.y + 1, curr_node.pos_.z);
            if (isValidPosition(curr_pos) && canMoveDiagonal)
            {
                if ((walls & (north | west)) == (north | west))
                {
                    childNode(curr_node, curr_pos, true);
                    //std::cout << "North West Square" << std::endl;
                }
            }

            // place parent node on closed list
            curr_node.list_ = B03_Node.OnList.ClosedList;
            if (DebugColoring) DebugVisusal.SetTile(curr_node.pos_, closedList);
            closedList_.Add(curr_node);
            openList_.Remove(curr_node);

            // if taken too much time or in single step abort shearch for now and resume next frame
            if (SingleStep == true)
            {
                timer = StepTime;
                return PathResult.PROCESSING;
            }
        }

        Debug.LogWarning("The path was computed as impossable.");
        return PathResult.IMPOSSIBLE;
    }

    bool isValidPosition(Vector3Int position)
    {
        bool valid = true;

        for(int i = 0; i < NonCollidibles.Count && valid; ++i)
        {
            if (NonCollidibles[i].HasTile(position)) valid = false;
        }

        return valid;
    }

    void childNode(B03_Node prev_node, Vector3Int position, bool isDiagonal = false)
    {
        // compute cost f(x) = g(x) + h(x)
        B03_Node curr_node = null;
        for (int i = 0; i < openList_.Count; ++i) if (openList_[i].pos_ == position) curr_node = openList_[i];
        for (int i = 0; i < closedList_.Count; ++i) if (closedList_[i].pos_ == position) curr_node = closedList_[i];
        if (curr_node == null) curr_node = new B03_Node();
        // heuristicscost and the giving cost
        float givencost = 0.0f;
        float heuristiccost = 0.0f;
        if (isDiagonal) givencost = prev_node.givenCost_ + 1.4142f;
        else givencost = prev_node.givenCost_ + 1.0f;

        heuristiccost = calulateHeuristics(position);

        // if child isn't on open or closed list put on open
        if (curr_node.list_ == B03_Node.OnList.Invalid)
        {
            openList_.Add(curr_node);
            curr_node.Update(prev_node, position, givencost);
            curr_node.heuristicCost_ = heuristiccost;
            if (DebugColoring) DebugVisusal.SetTile(curr_node.pos_, openList);
        }
        // else if child is on open or closed list and new one is cheaper
        else if (curr_node.heuristicCost_ + curr_node.givenCost_ > givencost + heuristiccost)
        {
            //  then take off old expensive one and put new cheaper one on Open list
            curr_node.Update(prev_node, position, givencost);
            if (DebugColoring) DebugVisusal.SetTile(curr_node.pos_, openList);
        }
    }

    float calulateHeuristics(Vector3Int curr_pos)
    {
        if (Heuristic == HeuristicType.OCTILE)
        {
            float delta_col = Mathf.Abs(curr_pos.x - currGoal.x);
            float delta_row = Mathf.Abs(curr_pos.y - currGoal.y);
            return HeuristicWeight * Mathf.Min(delta_col, delta_row) * 1.4142f + Mathf.Max(delta_col, delta_row) - Mathf.Min(delta_col, delta_row);
        }
        else if (Heuristic == HeuristicType.CHEBYSHEV)
        {
            float delta_col = Mathf.Abs(curr_pos.x - currGoal.x);
            float delta_row = Mathf.Abs(curr_pos.y - currGoal.y);
            return HeuristicWeight * Mathf.Max(delta_col, delta_row);
        }
        else if (Heuristic == HeuristicType.MANHATTAN)
        {
            float delta_col = Mathf.Abs(curr_pos.x - currGoal.x);
            float delta_row = Mathf.Abs(curr_pos.y - currGoal.y);
            return HeuristicWeight * (float)delta_col + delta_row;
        }
        else if (Heuristic == HeuristicType.EUCLIDEAN)
        {
            float delta_col = Mathf.Abs(curr_pos.x - currGoal.x);
            float delta_row = Mathf.Abs(curr_pos.y - currGoal.y);
            return HeuristicWeight * Mathf.Sqrt((float)delta_col * delta_col + delta_row * delta_row);
        }

        return 0;
    }
}
