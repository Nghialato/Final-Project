using _Scripts.GameCore.MovementSys;
using UnityEngine;

namespace _Scripts.GameCore.Entity.Enemy
{
    public class EnemyMovement : MovementComponent
    {
        public override void MoveUpdate()
        {
            transform.position = ComponentData.position;
            var angle = transform.eulerAngles;
            angle.z = ComponentData.rotation;
            transform.eulerAngles = angle;
        }
    }
}