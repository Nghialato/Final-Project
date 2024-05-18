using _Scripts.GameCore.HealthSys;
using UnityEngine;

namespace _Scripts.GameCore.Entity
{
    public class PlayerHealth : HealthEntity
    {
        public override void HealthUpdate()
        {
            Debug.Log($"Player Health {gameObject.name}");
        }
    }
}