using _Scripts.GameCore.MovementSys;
using UnityEngine;

public class CameraMovement : MovementEntity
{
    public override void Move()
    {
        _transform.position = positionData.position + new Vector3(0, 0, -10);
    }

    public override void RemoveFromSystem()
    {
        
    }
}
