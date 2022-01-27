using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_Node
{
    public enum OnList
    {
        Invalid,
        OpenList,
        ClosedList
    }

    public void Set(Vector3Int pos, float cost)
    {
        pos_ = new Vector3Int(pos.x, pos.y, pos.z);
        givenCost_ = cost;
    }

    public void Update(B03_Node prev_node, Vector3Int pos, float given_cost)
    {
        //void AStarPather::updateNode(Node& node, Node* prev_node, GridPos pos, float given_cost)
        prevNode_ = prev_node;
        list_ = OnList.OpenList;
        pos_ = pos;
        givenCost_ = given_cost;
    }

    public B03_Node prevNode_ = null;
    public Vector3Int pos_;
    public float heuristicCost_ = 0.0f;
    public float givenCost_ = 0.0f;
    public OnList list_ = OnList.Invalid;
}
