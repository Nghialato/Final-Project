using _Scripts.GameData;
using UnityEngine;

namespace _Scripts.GameCore.HealthSys
{
    public abstract class HealthComponent : BaseComponent<HealthData, HealthSystemManager>
    {
        public override void UpdateComponent()
        {
            if (ComponentData.dirty == false) return;
            HealthUpdate();
            ComponentData.dirty = false;
        }

        protected abstract void HealthUpdate();
    }
}