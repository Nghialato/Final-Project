using _Scripts.GameCore.HealthSys;
using _Scripts.GameCore.MovementSys;
using UnityEngine;

namespace _Scripts.GameCore.Entity
{
    public class PlayerMovement: MovementEntity
    {
        public override void Move()
        {
            Debug.Log("Player Move");
        }
    }
}