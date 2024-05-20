using _Scripts.GameCore.MovementSys;

namespace _Scripts.GameCore.Entity.Bullet
{
    public class BulletMovement : MovementEntity
    {
        public override void Move()
        {
            _transform.position = positionData.position;
        }
    }
}