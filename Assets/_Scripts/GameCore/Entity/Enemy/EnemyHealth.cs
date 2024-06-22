using _Scripts.GameCore.HealthSys;
using UnityEngine;

namespace _Scripts.GameCore.Entity.Enemy
{
    public class EnemyHealth : HealthComponent
    {
        public override void HealthUpdate()
        {
            if (ComponentData.health < 0)
            {
                Debug.Log($"Enemy death");
                gameObject.SetActive(false);
            }
        }
    }
}