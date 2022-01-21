using UnityEngine;

public class HyungseobKim_MovingTile_Vertical : HyungseobKim_MovingTile
{
    protected override void Move(float posToMove)
    {
        Vector3 pos = gameObject.transform.position;

        pos.y = posToMove;

        gameObject.transform.position = pos;
    }

    protected override float GetOriginalPos()
    {
        return gameObject.transform.position.y;
    }
}
