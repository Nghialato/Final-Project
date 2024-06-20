using _Scripts.GameCore.MovementSys;
using UnityEngine;

public class CameraMovement : MovementComponent
{
    public override void Move()
    {
        _transform.position = positionData.position + new Vector3(0, 0, -10);
    }

    public override void RemoveFromSystem()
    {
        
    }
}
