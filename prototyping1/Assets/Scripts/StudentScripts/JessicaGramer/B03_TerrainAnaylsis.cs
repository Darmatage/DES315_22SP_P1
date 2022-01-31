using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class B03_1DFloatArray
{
    [SerializeField] public float[] Array = null;

    public B03_1DFloatArray(int size)
    {
        Array = new float[size];
    }
}

[System.Serializable]
public class B03_2DLayer
{
    [SerializeField] private B03_1DFloatArray[] Layer = null;
    [SerializeField]
    public Vector3Int Size
    {
        get; private set;
    }

    public B03_2DLayer(Vector3Int size)
    {
        Layer = new B03_1DFloatArray[size.x];
        for (int i = 0; i < size.x; ++i) Layer[i] = new B03_1DFloatArray(size.y);

        Size = size;
    }

    public float this[int x, int y]
    {
        get
        {
            return Layer[x].Array[y];
        }
        set
        {
            Layer[x].Array[y] = value;
        }
    }

}


public class B03_TerrainAnaylsis : MonoBehaviour
{
    [SerializeField] Tilemap visibilityBlockers = null;
    [Header("Openness Layer")]
    [SerializeField] B03_2DLayer opennessLayer = null;
    [SerializeField] Tilemap opennessTilemap = null;
    [SerializeField] bool showOpenness = false;
    [SerializeField] Vector3 opennessColor = new Vector3(0.0f, 0.0f, 1.0f);
    bool prev_openness = false;

    [Header("Visibility Layer")]
    [SerializeField] B03_2DLayer visibilityLayer = null;
    [SerializeField] Tilemap visibilityTilemap = null;
    [SerializeField] bool showVisibility = false;
    [SerializeField] Vector3 visibilityColor = new Vector3(1.0f, 0.0f, 0.0f);
    [SerializeField] float visibilityMagicNumber = 160.0f;
    bool prev_visibility = false;

    Vector3Int zeroPosition;

    const int NORTH = 0b1000;
    const int EAST = 0b0100;
    const int SOUTH = 0b0010;
    const int WEST = 0b0001;

    const int NORTH_EAST = 0b1100;
    const int SOUTH_EAST = 0b0110;
    const int SOUTH_WEST = 0b0011;
    const int NORTH_WEST = 0b1001;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(prev_openness != showOpenness || showVisibility != prev_visibility)
        {
            UpdateDebugColoring();
        }
    }

    [ContextMenu("UpdateDebugColoring")]
    private void UpdateDebugColoring()
    {
        zeroPosition = visibilityBlockers.cellBounds.min;
        foreach (Vector3Int position in visibilityBlockers.cellBounds.allPositionsWithin)
        {
            Vector3Int curr_pos = position - zeroPosition;
            if (showOpenness && opennessLayer != null)
            {
                opennessTilemap.SetTileFlags(position, TileFlags.None);
                opennessTilemap.SetColor(position, new Color(opennessColor.x, opennessColor.y, opennessColor.z, opennessLayer[curr_pos.x, curr_pos.y]));
            }
            else
            {
                opennessTilemap.SetTileFlags(position, TileFlags.None);
                opennessTilemap.SetColor(position, new Color(opennessColor.x, opennessColor.y, opennessColor.z, 0.0f));
            }

            if (showVisibility && visibilityLayer != null)
            {
                visibilityTilemap.SetTileFlags(position, TileFlags.None);
                visibilityTilemap.SetColor(position, new Color(visibilityColor.x, visibilityColor.y, visibilityColor.z, visibilityLayer[curr_pos.x, curr_pos.y]));
            }
            else
            {
                visibilityTilemap.SetTileFlags(position, TileFlags.None);
                visibilityTilemap.SetColor(position, new Color(visibilityColor.x, visibilityColor.y, visibilityColor.z, 0.0f));
            }
        }
    }

    [ContextMenu("AnalyzeOpenness")]
    public void AnalyzeOpenness()
    {
        zeroPosition = visibilityBlockers.cellBounds.min;
        opennessLayer = new B03_2DLayer(visibilityBlockers.cellBounds.size);

        foreach (Vector3Int position in visibilityBlockers.cellBounds.allPositionsWithin)
        {

            float distance = distanceClosestWall(position);
            Vector3Int curr_pos = position - zeroPosition;
            if (distance == 0) opennessLayer[curr_pos.x, curr_pos.y] = 0.0f;
            else opennessLayer[curr_pos.x, curr_pos.y] = 1.0f / distance;

            if (showOpenness)
            {
                opennessTilemap.SetTileFlags(position, TileFlags.None);
                opennessTilemap.SetColor(position, new Color(opennessColor.x, opennessColor.y, opennessColor.z, opennessLayer[curr_pos.x, curr_pos.y]));
            }
        }
    }

    [ContextMenu("AnalyzeVisibility")]
    public void AnalyzeVisibility()
    {
        zeroPosition = visibilityBlockers.cellBounds.min;
        visibilityLayer = new B03_2DLayer(visibilityBlockers.cellBounds.size);

        foreach (Vector3Int position0 in visibilityBlockers.cellBounds.allPositionsWithin)
        {
            Vector3Int curr_pos = position0 - zeroPosition;
            int count = 0;

            foreach (Vector3Int position1 in visibilityBlockers.cellBounds.allPositionsWithin)
            {
                if(position0 != position1)
                {
                    if (isClearPath(position0, position1)) ++count;
                }
            }

            visibilityLayer[curr_pos.x, curr_pos.y] = Mathf.Min(count / visibilityMagicNumber, 1.0f);

            if (showVisibility)
            {
                visibilityTilemap.SetTileFlags(position0, TileFlags.None);
                visibilityTilemap.SetColor(position0, new Color(visibilityColor.x, visibilityColor.y, visibilityColor.z, visibilityLayer[curr_pos.x, curr_pos.y]));
            }
        }
    }

    float distanceClosestWall(Vector3Int position)
    {
        float distance = Mathf.Max(Mathf.Abs(position.x) + 1, Mathf.Abs(position.y + 1));
        if (visibilityBlockers.HasTile(position)) return 0.0f;

        foreach (Vector3Int xy in visibilityBlockers.cellBounds.allPositionsWithin)
        {
            Vector3 delta = xy - position;
            if (visibilityBlockers.HasTile(xy))
                distance = Mathf.Min(Mathf.Sqrt((delta.x * delta.x) + (delta.y * delta.y) + (delta.z * delta.z)), distance);
        }

        return distance;
    }

    bool isClearPath(Vector3Int position0, Vector3Int position1)
    {
        /*
            Two cells (row0, col0) and (row1, col1) are visible to each other if a line
            between their centerpoints doesn't intersect the four boundary lines of every
            wall cell.  You should puff out the four boundary lines by a very tiny amount
            so that a diagonal line passing by the corner will intersect it.  Make use of the
            line_intersect helper function for the intersection test and the is_wall member
            function in the global terrain to determine if a cell is a wall or not.
        */
    
        // WRITE YOUR CODE HERE
    
        float distance = Vector3.Distance(visibilityBlockers.CellToWorld(position0), visibilityBlockers.CellToWorld(position1));
    
        Vector3 point_one = visibilityBlockers.CellToWorld(position0);
        Vector3 point_two = visibilityBlockers.CellToWorld(position1);
    
        Vector3Int min = new Vector3Int(Mathf.Min(position0.x, position1.x), Mathf.Min(position0.y, position1.y), Mathf.Min(position0.z, position1.z));
        Vector3Int max = new Vector3Int(Mathf.Max(position0.x, position1.x), Mathf.Max(position0.y, position1.y), Mathf.Max(position0.z, position1.z));

        // loop though row
        BoundsInt bounds = visibilityBlockers.cellBounds;
        bounds.SetMinMax(min, max);
        for(int x = min.x; x <= max.x; ++x)
            for(int y = min.y; y <= max.y; ++y)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                if (visibilityBlockers.HasTile(position))
                {
                    Vector3 wall = visibilityBlockers.CellToWorld(position);
                    Vector3 wall_center = wall;

                    Vector3 lower_left = wall_center;
                    lower_left.x -= distance / visibilityBlockers.size.x;
                    lower_left.y -= distance / visibilityBlockers.size.y;
                    Vector3 upper_left = wall_center;
                    upper_left.x -= distance / visibilityBlockers.size.x;
                    upper_left.y += distance / visibilityBlockers.size.y;

                    Vector3 lower_right = wall_center;
                    lower_right.x += distance / visibilityBlockers.size.x;
                    lower_right.y -= distance / visibilityBlockers.size.y;
                    Vector3 upper_right = wall_center;
                    upper_right.x += distance / visibilityBlockers.size.x;
                    upper_right.y += distance / visibilityBlockers.size.y;

                    if (lineIntersect(point_one, point_two, upper_left, upper_right)) return false;
                    if (lineIntersect(point_one, point_two, upper_right, lower_right)) return false;
                    if (lineIntersect(point_one, point_two, lower_right, lower_left)) return false;
                    if (lineIntersect(point_one, point_two, lower_left, upper_left)) return false;
                }
            }
    
        return true;
    }

    bool lineIntersect(Vector3 point0_0, Vector3 point0_1, Vector3 point1_0, Vector3 point1_1)
    {
        Vector3 line0 = point0_1 - point0_0;
        Vector3 line1 = point1_1 - point1_0;

        // parallel lines
        Vector3 normal0 = Vector3.Normalize(line0);
        Vector3 normal1 = Vector3.Normalize(line1);
        if (normal0 == normal1)
        {
            Vector3 other_line = point1_0 - point0_0;
            if (Vector3.Normalize(other_line) == normal0) return true;
            else return false;
        }

        // non-parallel lines

        float s = (point1_0.x - point0_0.x + (line1.x / line1.y) * (point1_0.x - point0_0.x)) / (line0.x - (line0.y * line1.x / line0.x));
        float t = (point0_0.y - point1_0.y + (line0.y / line0.x) * (point1_0.x - point0_0.x)) / (line1.y - (line1.x * line0.y / line0.x));

        // check if point of intersection exists
        if (point0_0.z + s * line0.z != point1_0.z + t * line1.z) return false;

        // get the intersection
        Vector3 intersection_point = point0_0 + s * line0;

        // check if on first line
        if (Vector3.Dot(point0_1 - point0_0, intersection_point - point0_0) < 0) return false;
        // check if on second line
        if (Vector3.Dot(point1_1 - point1_0, intersection_point - point1_0) < 0) return false;

        return true;
    }

    public B03_2DLayer AnalyzeVisibleToCell(Vector3Int position)
    {
        /*
            For every cell in the given layer mark it with 1.0 if it is visible to the given cell, 
            0.5 if it isn't visible but is next to a visible cell,
            or 0.0 otherwise.

            Two cells are visible to each other if a line between their centerpoints doesn't
            intersect the four boundary lines of every wall cell.  Make use of the is_clear_path
            helper function.
        */
        B03_2DLayer layer = new B03_2DLayer(visibilityBlockers.cellBounds.size);

        foreach(var position0 in visibilityBlockers.cellBounds.allPositionsWithin)
        {
            Vector3Int curr_pos = position0 - zeroPosition;
            if (isClearPath(position, position0)) layer[curr_pos.x, curr_pos.y] = 1.0f;
            else if (!visibilityBlockers.HasTile(position0))
            {
                bool is_set = false;
                int walls = 0b1111;

                // check NORTH
                Vector3Int n_position = new Vector3Int(position0.x, position0.y + 1, position0.z);
                if (checkDirection(ref walls, position0, NORTH) && isClearPath(position, n_position))
                {
                    layer[curr_pos.x, curr_pos.y] = 0.5f;
                    is_set = true;
                }
                // check EAST
                n_position.Set(position0.x + 1, position0.y, position0.z);
                if (checkDirection(ref walls, position0, EAST) && isClearPath(position, n_position))
                {
                    layer[curr_pos.x, curr_pos.y] = 0.5f;
                    is_set = true;
                }
                // check SOUTH
                n_position.Set(position0.x, position0.y - 1, position0.z);
                if (checkDirection(ref walls, position0, SOUTH) && isClearPath(position, n_position))
                {
                    layer[curr_pos.x, curr_pos.y] = 0.5f;
                    is_set = true;
                }
                // check WEST
                n_position.Set(position0.x - 1, position0.y, position0.z);
                if (checkDirection(ref walls, position0, WEST) && isClearPath(position, n_position))
                {
                    layer[curr_pos.x, curr_pos.y] = 0.5f;
                    is_set = true;
                }

                // check NORTH EAST
                n_position.Set(position0.x + 1, position0.y + 1, position0.z);
                if (checkDirection(ref walls, position0, NORTH_EAST) && isClearPath(position, n_position))
                {
                    layer[curr_pos.x, curr_pos.y] = 0.5f;
                    is_set = true;
                }
                // SOUTH EAST
                n_position.Set(position0.x + 1, position0.y - 1, position0.z);
                if (checkDirection(ref walls, position0, SOUTH_EAST) && isClearPath(position, n_position))
                {
                    layer[curr_pos.x, curr_pos.y] = 0.5f;
                    is_set = true;
                }
                // SOUTH WEST
                n_position.Set(position0.x - 1, position0.y - 1, position0.z);
                if (checkDirection(ref walls, position0, SOUTH_WEST) && isClearPath(position, n_position))
                {
                    layer[curr_pos.x, curr_pos.y] = 0.5f;
                    is_set = true;
                }
                // NORTH WEST
                n_position.Set(position0.x - 1, position0.y + 1, position0.z);
                if (checkDirection(ref walls, position0, NORTH_WEST) && isClearPath(position, n_position))
                {
                    layer[curr_pos.x, curr_pos.y] = 0.5f;
                    is_set = true;
                }

                if (!is_set) layer[curr_pos.x, curr_pos.y] = 0.0f;
            }
        }

        return layer;
    }

    void setWall(ref int walls, int wall_to_set)
    {
      walls = walls ^ wall_to_set;
    }

    bool checkWall(int walls, int wall_to_check)
    {
        return (walls & wall_to_check) == wall_to_check;
    }

    bool checkDirection(ref int walls, Vector3Int curr_pos, int direction)
    {
        int n_x = curr_pos.x;
        int n_y = curr_pos.y;

        if (direction == NORTH)
        {
            n_y += 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (visibilityBlockers.HasTile(pos)) setWall(ref walls, NORTH);
            else return true;
        }
        else if (direction == EAST)
        {
            n_x += 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (visibilityBlockers.HasTile(pos)) setWall(ref walls, EAST);
            else return true;
        }
        else if (direction == SOUTH)
        {
            n_y -= 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (visibilityBlockers.HasTile(pos)) setWall(ref walls, SOUTH);
            else return true;
        }
        else if (direction == WEST)
        {
            n_x -= 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (visibilityBlockers.HasTile(pos)) setWall(ref walls, WEST);
            else return true;
        }

        else if (direction == NORTH_EAST)
        {
            n_x += 1; n_y += 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (checkWall(walls, NORTH_EAST)) return true;
        }
        else if (direction == SOUTH_EAST)
        {
            n_x += 1; n_y -= 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (checkWall(walls, SOUTH_EAST)) return true;
        }
        else if (direction == SOUTH_WEST)
        {
            n_x -= 1; n_y -= 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (checkWall(walls, SOUTH_WEST)) return true;
        }
        else if (direction == NORTH_WEST)
        {
            n_x -= 1; n_y += 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (checkWall(walls, NORTH_WEST)) return true;
        }
        return false;
    }
}
