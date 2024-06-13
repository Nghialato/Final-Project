using _Scripts.GameCore.MovementSys;

namespace _Scripts.GameCore.Entity
{
    public class PlayerMovement: MovementEntity
    {
        public override void Move()
        {
            _transform.position = positionData.position;
            var angle = _transform.eulerAngles;
            angle.z = positionData.rotation;
            _transform.eulerAngles = angle;
        }
    }
}