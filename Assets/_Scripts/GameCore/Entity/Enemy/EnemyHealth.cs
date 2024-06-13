using _Scripts.GameCore.HealthSys;
using UnityEngine;

namespace _Scripts.GameCore.Entity.Enemy
{
    public class EnemyHealth : HealthEntity
    {
        public override void HealthUpdate()
        {
            if (healthData.health < 0)
            {
                Debug.Log($"Enemy death");
                gameObject.SetActive(false);
            }
        }
    }
}