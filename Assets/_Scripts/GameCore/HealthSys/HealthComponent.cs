using UnityEngine;

namespace _Scripts.GameCore.HealthSys
{
    public abstract class HealthComponent : BaseComponent<HealthData, HealthSystemManager>
    {
        public void UpdateComponent()
        {
            if (ComponentData.dirty == false) return;
            HealthUpdate();
            ComponentData.dirty = false;
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