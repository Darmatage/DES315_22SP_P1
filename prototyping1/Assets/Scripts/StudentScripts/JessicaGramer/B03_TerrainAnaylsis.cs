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
    [SerializeField] public Tilemap VisibilityBlockers = null;
    [Header("Openness Layer")]
    [SerializeField] B03_2DLayer opennessLayer = null;
    [SerializeField] Tilemap opennessTilemap = null;
    [SerializeField] bool showOpenness = false;
    [SerializeField] Vector3 opennessColor = new Vector3(0.0f, 0.0f, 1.0f);

    [Header("Visibility Layer")]
    [SerializeField] B03_2DLayer visibilityLayer = null;
    [SerializeField] Tilemap visibilityTilemap = null;
    [SerializeField] bool showVisibility = false;
    [SerializeField] Vector3 visibilityColor = new Vector3(1.0f, 0.0f, 0.0f);
    [SerializeField] float visibilityMagicNumber = 160.0f;

    public Vector3Int ZeroPosition;

    const int NORTH = 0b1000;
    const int EAST = 0b0100;
    const int SOUTH = 0b0010;
    const int WEST = 0b0001;

    const int NORTH_EAST = 0b1100;
    const int SOUTH_EAST = 0b0110;
    const int SOUTH_WEST = 0b0011;
    const int NORTH_WEST = 0b1001;

    private void Start()
    {
        ZeroPosition = VisibilityBlockers.cellBounds.min;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("UpdateDebugColoring")]
    private void UpdateDebugColoring()
    {
        ZeroPosition = VisibilityBlockers.cellBounds.min;
        foreach (Vector3Int position in VisibilityBlockers.cellBounds.allPositionsWithin)
        {
            Vector3Int curr_pos = position - ZeroPosition;
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
        ZeroPosition = VisibilityBlockers.cellBounds.min;
        opennessLayer = new B03_2DLayer(VisibilityBlockers.cellBounds.size);

        foreach (Vector3Int position in VisibilityBlockers.cellBounds.allPositionsWithin)
        {

            float distance = DistanceClosestWall(position);
            Vector3Int curr_pos = position - ZeroPosition;
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
        ZeroPosition = VisibilityBlockers.cellBounds.min;
        visibilityLayer = new B03_2DLayer(VisibilityBlockers.cellBounds.size);

        foreach (Vector3Int position0 in VisibilityBlockers.cellBounds.allPositionsWithin)
        {
            Vector3Int curr_pos = position0 - ZeroPosition;
            int count = 0;

            foreach (Vector3Int position1 in VisibilityBlockers.cellBounds.allPositionsWithin)
            {
                if(position0 != position1)
                {
                    if (IsClearPath(position0, position1)) ++count;
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

    public void NormalizeSoloOccupancy(B03_2DLayer layer)
    {
        /*
            Determine the maximum value in the given layer, and then divide the value
            for every cell in the layer by that amount.  This will keep the values in the
            range of [0, 1].  Negative values should be left unmodified.
        */

        // WRITE YOUR CODE HERE
        float max_value = 0.0f;

        // find max value
        foreach(Vector3Int position in VisibilityBlockers.cellBounds.allPositionsWithin)
        {
            max_value = Mathf.Max(layer[position.x - ZeroPosition.x, position.y - ZeroPosition.y], max_value);
        }

        // divide the value
        foreach (Vector3Int position in VisibilityBlockers.cellBounds.allPositionsWithin)
        {
            Vector3Int curr_position = position - ZeroPosition;
            {
                //max_value = std::max(layer.get_value(row, col), max_value);
                float curr_value = layer[curr_position.x, curr_position.y];
                if (curr_value > 0) curr_value /= max_value;
                layer[curr_position.x, curr_position.y] = curr_value;
            }
        }
    }

    float DistanceClosestWall(Vector3Int position)
    {
        float distance = Mathf.Max(Mathf.Abs(position.x) + 1, Mathf.Abs(position.y + 1));
        if (VisibilityBlockers.HasTile(position)) return 0.0f;

        foreach (Vector3Int xy in VisibilityBlockers.cellBounds.allPositionsWithin)
        {
            Vector3 delta = xy - position;
            if (VisibilityBlockers.HasTile(xy))
                distance = Mathf.Min(Mathf.Sqrt((delta.x * delta.x) + (delta.y * delta.y) + (delta.z * delta.z)), distance);
        }

        return distance;
    }

    bool IsClearPath(Vector3Int position0, Vector3Int position1)
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
    
        float distance = Vector3.Distance(VisibilityBlockers.CellToWorld(position0), VisibilityBlockers.CellToWorld(position1));
    
        Vector3 point_one = VisibilityBlockers.CellToWorld(position0);
        Vector3 point_two = VisibilityBlockers.CellToWorld(position1);
    
        Vector3Int min = new Vector3Int(Mathf.Min(position0.x, position1.x), Mathf.Min(position0.y, position1.y), Mathf.Min(position0.z, position1.z));
        Vector3Int max = new Vector3Int(Mathf.Max(position0.x, position1.x), Mathf.Max(position0.y, position1.y), Mathf.Max(position0.z, position1.z));

        // loop though row
        BoundsInt bounds = VisibilityBlockers.cellBounds;
        bounds.SetMinMax(min, max);
        for(int x = min.x; x <= max.x; ++x)
            for(int y = min.y; y <= max.y; ++y)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                if (VisibilityBlockers.HasTile(position))
                {
                    Vector3 wall = VisibilityBlockers.CellToWorld(position);
                    Vector3 wall_center = wall;

                    Vector3 lower_left = wall_center;
                    lower_left.x -= distance / VisibilityBlockers.size.x;
                    lower_left.y -= distance / VisibilityBlockers.size.y;
                    Vector3 upper_left = wall_center;
                    upper_left.x -= distance / VisibilityBlockers.size.x;
                    upper_left.y += distance / VisibilityBlockers.size.y;

                    Vector3 lower_right = wall_center;
                    lower_right.x += distance / VisibilityBlockers.size.x;
                    lower_right.y -= distance / VisibilityBlockers.size.y;
                    Vector3 upper_right = wall_center;
                    upper_right.x += distance / VisibilityBlockers.size.x;
                    upper_right.y += distance / VisibilityBlockers.size.y;

                    if (LineIntersect(point_one, point_two, upper_left, upper_right)) return false;
                    if (LineIntersect(point_one, point_two, upper_right, lower_right)) return false;
                    if (LineIntersect(point_one, point_two, lower_right, lower_left)) return false;
                    if (LineIntersect(point_one, point_two, lower_left, upper_left)) return false;
                }
            }
    
        return true;
    }

    bool LineIntersect(Vector3 point0_0, Vector3 point0_1, Vector3 point1_0, Vector3 point1_1)
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

    void SetWall(ref int walls, int wall_to_set)
    {
      walls ^= wall_to_set;
    }

    bool CheckWall(int walls, int wall_to_check)
    {
        return (walls & wall_to_check) == wall_to_check;
    }

    bool CheckDirection(ref int walls, Vector3Int curr_pos, int direction)
    {
        int n_x = curr_pos.x;
        int n_y = curr_pos.y;

        if (direction == NORTH)
        {
            n_y += 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (VisibilityBlockers.HasTile(pos)) SetWall(ref walls, NORTH);
            else return true;
        }
        else if (direction == EAST)
        {
            n_x += 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (VisibilityBlockers.HasTile(pos)) SetWall(ref walls, EAST);
            else return true;
        }
        else if (direction == SOUTH)
        {
            n_y -= 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (VisibilityBlockers.HasTile(pos)) SetWall(ref walls, SOUTH);
            else return true;
        }
        else if (direction == WEST)
        {
            n_x -= 1;
            Vector3Int pos = new Vector3Int(n_x, n_y, 0);
            if (VisibilityBlockers.HasTile(pos)) SetWall(ref walls, WEST);
            else return true;
        }

        else if (direction == NORTH_EAST)
        {
            if (CheckWall(walls, NORTH_EAST)) return true;
        }
        else if (direction == SOUTH_EAST)
        {
            if (CheckWall(walls, SOUTH_EAST)) return true;
        }
        else if (direction == SOUTH_WEST)
        {
            if (CheckWall(walls, SOUTH_WEST)) return true;
        }
        else if (direction == NORTH_WEST)
        {
            if (CheckWall(walls, NORTH_WEST)) return true;
        }
        return false;
    }
}
