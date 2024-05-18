using _Scripts.GameCore.MovementSys;
using UnityEngine;

namespace _Scripts.GameCore.Entity.Enemy
{
    public class EnemyMovement : MovementEntity
    {
        public override void Move()
        {
            _transform.position = positionData.position;
        }
    }
}