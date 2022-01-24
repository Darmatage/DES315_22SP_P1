using UnityEngine;

public class HyungseobKim_MovingTile_Horizontal : HyungseobKim_MovingTile
{
    protected override void Move(float posToMove)
    {
        Vector3 pos = gameObject.transform.position;

        pos.x = posToMove;

        gameObject.transform.position = pos;
    }

    protected override float GetOriginalPos()
    {
        return gameObject.transform.position.x;
    }
}
