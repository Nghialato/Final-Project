using _Scripts.GameCore.MovementSys;
using UnityEngine;

public class CameraMovement : MovementComponent
{
    public override void MoveUpdate()
    {
        _transform.position = ComponentData.position + new Vector3(0, 0, -10);
    }
}
