using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class B03_TerrainAnaylsis : MonoBehaviour
{
    [SerializeField] Tilemap opennessTilemap = null;
    [SerializeField] Tilemap visibilityTilemap = null;
    [SerializeField] Tilemap visibilityBlockers = null;
    [SerializeField] bool showOpenness = false;
    [SerializeField] bool showVisibility = false;
    [SerializeField] Vector3 opennessColor = new Vector3(0.0f, 0.0f, 1.0f);
    [SerializeField] Vector3 visibilityColor = new Vector3(1.0f, 0.0f, 0.0f);

    bool prevOpen = true;
    bool prevVis = true;

    //Vector2Int layerSize = new Vector2Int();
    [SerializeField] SO_Layer opennessLayer;
    [SerializeField] SO_Layer visibilityLayer;

    Vector3Int tilemapZero;

    // Start is called before the first frame update
    void Start()
    {
        //BoundsInt bounds = visibilityBlockers[0].cellBounds;

        // Get the largest layer size
        opennessLayer.Load();
        visibilityLayer.Load();

        prevOpen = !showOpenness;
        prevVis = !showVisibility;
        tilemapZero = visibilityBlockers.cellBounds.min;
    }

    [ContextMenu("DisplayLayers")]
    // Update is called once per frame
    void Update()
    {
        if(prevOpen != showOpenness && opennessLayer.Layer != null)
        {
            foreach (Vector3Int position in visibilityBlockers.cellBounds.allPositionsWithin)
            {
                opennessTilemap.SetTileFlags(position, TileFlags.None);
                if (showOpenness) opennessTilemap.SetColor(position, new 
                    Color(opennessColor.x, opennessColor.y, opennessColor.z, opennessLayer.Layer[position.x - tilemapZero.x, position.y - tilemapZero.y]));
                else opennessTilemap.SetColor(position, new Color(opennessColor.x, opennessColor.y, opennessColor.z, 0.0f));
            }

            prevOpen = showOpenness;
        }

        if(prevVis != showVisibility && visibilityLayer.Layer != null)
        {
            foreach (Vector3Int position in visibilityBlockers.cellBounds.allPositionsWithin)
            {
                visibilityTilemap.SetTileFlags(position, TileFlags.None);
                if (showVisibility) visibilityTilemap.SetColor(position, new 
                    Color(visibilityColor.x, visibilityColor.y, visibilityColor.z, visibilityLayer.Layer[position.x - tilemapZero.x, position.y - tilemapZero.y]));
                else visibilityTilemap.SetColor(position, new Color(visibilityColor.x, visibilityColor.y, visibilityColor.z, 0.0f));
            }

            prevVis = showVisibility;
        }
    }

    [ContextMenu("AnalyzeOpenness")]
    public void AnalyzeOpenness()
    {
        tilemapZero = visibilityBlockers.cellBounds.min;
        opennessLayer.Layer = new float[visibilityBlockers.cellBounds.size.x, visibilityBlockers.cellBounds.size.y];
        foreach (Vector3Int position in visibilityBlockers.cellBounds.allPositionsWithin)
        {
            opennessTilemap.SetTileFlags(position, TileFlags.None);
            float distance = distanceClosestWall(position);
            if (distance != 0) opennessLayer.Layer[position.x - tilemapZero.x, position.y - tilemapZero.y] = 1.0f / distance;
            else opennessLayer.Layer[position.x - tilemapZero.x, position.y - tilemapZero.y] = 0.0f;

            if (showOpenness) 
                opennessTilemap.SetColor(position, new 
                    Color(opennessColor.x, opennessColor.y, opennessColor.z, opennessLayer.Layer[position.x - tilemapZero.x, position.y - tilemapZero.y]));
        }

        opennessLayer.Save();
    }

    [ContextMenu("AnalyzeVisibility")]
    public void AnalyzeVisibility()
    {
        tilemapZero = visibilityBlockers.cellBounds.min;
        visibilityLayer.Layer = new float[visibilityBlockers.cellBounds.size.x, visibilityBlockers.cellBounds.size.y];
        foreach (Vector3Int position0 in visibilityBlockers.cellBounds.allPositionsWithin)
        {
            visibilityTilemap.SetTileFlags(position0, TileFlags.None);
            float count = 0.0f;
            foreach (Vector3Int position1 in visibilityBlockers.cellBounds.allPositionsWithin)
            {
                if(position0 != position1)
                {
                    if (isClearPath(position0, position1)) ++count;
                }
            }

            visibilityLayer.Layer[position0.x - tilemapZero.x, position0.y - tilemapZero.y] = Mathf.Min(count / 160.0f, 1.0f);
            if (showVisibility) opennessTilemap.SetColor(position0, 
                new Color(visibilityColor.x, visibilityColor.y, visibilityColor.z, visibilityLayer.Layer[position0.x - tilemapZero.x, position0.y - tilemapZero.y]));
        }

        visibilityLayer.Save();
    }

    float distanceClosestWall(Vector3Int position)
    {
        float distance = Mathf.Min(Mathf.Abs(position.x) + 1, Mathf.Abs(position.y) + 1);
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
    
        float distance = Vector3.Distance(visibilityBlockers.CellToWorld(position0), visibilityBlockers.CellToWorld(position1));
    
        bool no_wall = true;
    
        Vector3 point_one = visibilityBlockers.CellToWorld(position0);
        Vector3 point_two = visibilityBlockers.CellToWorld(position1);
    
        Vector3Int min = new Vector3Int(Mathf.Min(position0.x, position1.x), Mathf.Min(position0.y, position1.y), Mathf.Min(position0.z, position1.z));
        Vector3Int max = new Vector3Int(Mathf.Max(position0.x, position1.x), Mathf.Max(position0.y, position1.y), Mathf.Max(position0.z, position1.z));

        for (int x = min.x; x <= max.x; ++x)
            for (int y = min.y; y <= max.y; ++y)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                if (visibilityBlockers.HasTile(position))
                {
                    // grab the wall position
                    Vector3 wall = visibilityBlockers.CellToWorld(position);

                    Vector3 upper_left = new Vector3();
                    upper_left.x -= distance / 1.89f;
                    upper_left.y -= distance / 1.89f;
                    Vector3 lower_left = new Vector3();
                    lower_left.x -= distance / 1.89f;
                    lower_left.y += distance / 1.89f;

                    Vector3 lower_right = new Vector3();
                    lower_right.x += distance / 1.89f;
                    lower_right.y -= distance / 1.89f;
                    Vector3 upper_right = new Vector3();
                    upper_right.x += distance / 1.89f;
                    upper_right.y += distance / 1.89f;

                    if (lineIntersect(point_one, point_two, upper_left, upper_right)) no_wall = false;
                    if (lineIntersect(point_one, point_two, upper_right, lower_right)) no_wall = false;
                    if (lineIntersect(point_one, point_two, lower_right, lower_left)) no_wall = false;
                    if (lineIntersect(point_one, point_two, lower_left, upper_left)) no_wall = false;
                }
            }
    
        return no_wall;
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

        // not parallel
        float s = (point1_0.x - point0_0.x + (line1.x / line1.y) * (point0_0.x - point1_0.y)) / (line0.x - (line0.y * line1.x) / line1.y);
        float t = (point0_0.y - point1_0.y - (line0.y / line0.x) * (point1_0.x - point0_0.x)) / (line1.y - (line1.x * line0.y) / line0.x);

        // if there is no intersection point
        if (point0_0.z + s * line0.z != point1_0.z + t * line1.z) return false;

        Vector3 intersection_point = point0_0 + (s * line0);

        // is point on line 0?
        if (!(point0_0.x <= intersection_point.x && point0_0.y <= intersection_point.y && point0_0.z <= intersection_point.z
            && point0_1.x >= intersection_point.x && point0_1.y >= intersection_point.y && point0_1.z >= intersection_point.z)) return false;

        // is point on line 1?
        if (!(point1_0.x <= intersection_point.x && point1_0.y <= intersection_point.y && point1_0.z <= intersection_point.z
            && point1_1.x >= intersection_point.x && point1_1.y >= intersection_point.y && point1_1.z >= intersection_point.z)) return false;

        return true;
    }
}
