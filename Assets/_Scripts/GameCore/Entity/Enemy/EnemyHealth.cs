using _Scripts.GameCore.HealthSys;
using UnityEngine;

namespace _Scripts.GameCore.Entity.Enemy
{
    public class EnemyHealth : HealthEntity
    {
        public override void HealthUpdate()
        {
            Debug.Log($"Enemy health {gameObject.name}");
        }
    }
}