using _Scripts.GameCore.MovementSys;

namespace _Scripts.GameCore.Entity
{
    public class PlayerMovement: MovementComponent
    {
        public override void MoveUpdate()
        {
            _transform.position = ComponentData.position;
            var angle = _transform.eulerAngles;
            angle.z = ComponentData.rotation;
            _transform.eulerAngles = angle;
        }
    }
}