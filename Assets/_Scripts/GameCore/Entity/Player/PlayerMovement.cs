using _Scripts.GameCore.MovementSys;
using UnityEngine;

namespace _Scripts.GameCore.Entity
{
    public class PlayerMovement: MovementEntity
    {
        public override void Move()
        {
            _transform.position = positionData.position;
        }
    }
}