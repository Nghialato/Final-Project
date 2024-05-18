using System;
using UnityEngine;

namespace _Scripts.GameCore.HealthSys
{
    public abstract class HealthEntity : MonoBehaviour, IHealth
    {
        public HealthData healthData;
        public void RegisterToSystem()
        {
            HealthSystemManagerEts.RegisterToArray(this);
        }

        public void RemoveFromSystem()
        {
            HealthSystemManagerEts.RemoveFromArray(this);
        }

        public void UpdateComponent()
        {
            if (healthData.dirty == false) return;
            HealthUpdate();
            healthData.dirty = false;
        }

        public virtual void HealthUpdate()
        {
            
        }

        private void OnDisable()
        {
            RemoveFromSystem();
        }
    }
}