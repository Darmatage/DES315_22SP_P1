using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class B03_TerrainAnaylsis : MonoBehaviour
{
    [SerializeField] Tilemap opennessTilemap = null;
    //[SerializeField] Tilemap visibilityTilemap = null;
    [SerializeField] Tilemap visibilityBlockers = null;
    [SerializeField] bool showOpenness = false;
    //[SerializeField] bool showVisibility = false;
    [SerializeField] Vector3 opennessColor = new Vector3(0.0f, 0.0f, 1.0f);
    //[SerializeField] Vector3 visibilityColor = new Vector3(1.0f, 0.0f, 0.0f);

    //Vector2Int layerSize = new Vector2Int();
    float[,] opennessLayer = null;
    float[,] visibilityLayer = null;
    
    // Start is called before the first frame update
    void Start()
    {
        //BoundsInt bounds = visibilityBlockers[0].cellBounds;

        // Get the largest layer size
        opennessLayer = new float[visibilityBlockers.cellBounds.size.x, visibilityBlockers.cellBounds.size.y];
        visibilityLayer = new float[visibilityBlockers.cellBounds.size.x, visibilityBlockers.cellBounds.size.y];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnalyzeOpenness()
    {
        foreach(Vector3Int position in visibilityBlockers.cellBounds.allPositionsWithin)
        {
            opennessLayer[position.x, position.y] = 1.0f / distanceClosestWall(position);
            if (showOpenness) opennessTilemap.SetColor(position, new Color(opennessColor.x, opennessColor.y, opennessColor.z, opennessLayer[position.x, position.y]));
        }
    }

    public void AnalyzeVisibility()
    {
        //
    }

    float distanceClosestWall(Vector3Int position)
    {
        float distance = Mathf.Min(position.x + 1, position.y + 1);
        if (visibilityBlockers.HasTile(position)) return 0.0f;

        foreach (Vector3Int xy in visibilityBlockers.cellBounds.allPositionsWithin)
        {
            float delta_x = xy.x - position.x;
            float delta_y = xy.y - position.y;
            float delta_z = xy.z - position.z;
            if (visibilityBlockers.HasTile(xy))
                distance = Mathf.Min(Mathf.Sqrt(delta_x * delta_x + delta_y * delta_y), distance);
        }

        return distance;
    }

    //bool is_clear_path(Vector3Int position0, Vector3Int position1)
    //{
    //    /*
    //        Two cells (row0, col0) and (row1, col1) are visible to each other if a line
    //        between their centerpoints doesn't intersect the four boundary lines of every
    //        wall cell.  You should puff out the four boundary lines by a very tiny amount
    //        so that a diagonal line passing by the corner will intersect it.  Make use of the
    //        line_intersect helper function for the intersection test and the is_wall member
    //        function in the global terrain to determine if a cell is a wall or not.
    //    */
    //
    //    // WRITE YOUR CODE HERE
    //
    //    float distance = Vector3.Distance(visibilityBlockers.CellToWorld(position0), visibilityBlockers.CellToWorld(position1));
    //
    //    bool no_wall = true;
    //    //const int num_col = col1 - col0;
    //    //const int num_row = row1 - row0;
    //    Vector3Int delta = position1 - position0;
    //
    //    Vec3 point_one = terrain->get_world_position(row0, col0);
    //    Vec2 v2_one;
    //    v2_one.x = point_one.x;
    //    v2_one.y = point_one.z;
    //
    //    Vec3 point_two = terrain->get_world_position(row1, col1);
    //    Vec2 v2_two;
    //    v2_two.x = point_two.x;
    //    v2_two.y = point_two.z;
    //
    //    // get min col, max col
    //    //int min_x = Mathf.Min(position1.x, position0.x), max_x = Mathf.Max(position1.x, position0.x);
    //    // get min row, max row
    //    //int min_row = std::min(row1, row0), max_row = std::max(row1, row0);
    //
    //    Vector3Int min = new Vector3Int(Mathf.Min(position0.x, position1.x), Mathf.Min(position0.y, position1.y), Mathf.Min(position0.z, position1.z));
    //    Vector3Int max = new Vector3Int(Mathf.Max(position0.x, position1.x), Mathf.Max(position0.y, position1.y), Mathf.Max(position0.z, position1.z));
    //
    //    // loop though row
    //    for (int x = min.x; x <= max.x; ++x)
    //    {
    //        for (int y = min.y; y <= max.y; ++y)
    //        {
    //            if (terrain->is_wall(x, y))
    //            {
    //                Vec3 wall = terrain->get_world_position(x, y);
    //                Vec2 v2_wall_center;
    //                v2_wall_center.x = wall.x;
    //                v2_wall_center.y = wall.z;
    //
    //                Vec2 wall_lower_left = v2_wall_center;
    //                wall_lower_left.x -= distance / 1.89f;
    //                wall_lower_left.y -= distance / 1.89f;
    //                Vec2 wall_upper_left = v2_wall_center;
    //                wall_upper_left.x -= distance / 1.89f;
    //                wall_upper_left.y += distance / 1.89f;
    //
    //                Vec2 wall_lower_right = v2_wall_center;
    //                wall_lower_right.x += distance / 1.89f;
    //                wall_lower_right.y -= distance / 1.89f;
    //                Vec2 wall_upper_right = v2_wall_center;
    //                wall_upper_right.x += distance / 1.89f;
    //                wall_upper_right.y += distance / 1.89f;
    //
    //                if (lineIntersect(v2_one, v2_two, wall_upper_left, wall_upper_right)) no_wall = false;
    //                if (lineIntersect(v2_one, v2_two, wall_upper_right, wall_lower_right)) no_wall = false;
    //                if (lineIntersect(v2_one, v2_two, wall_lower_right, wall_lower_left)) no_wall = false;
    //                if (lineIntersect(v2_one, v2_two, wall_lower_left, wall_upper_left)) no_wall = false;
    //            }
    //        }
    //    }
    //
    //    return no_wall;
    //}

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

        return false;
    }
}
