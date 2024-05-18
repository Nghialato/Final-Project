using System;
using UnityEngine;

namespace _Scripts.GameCore.HealthSys
{
    public abstract class HealthEntity : MonoBehaviour, IHealth
    {
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
            HealthUpdate();
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