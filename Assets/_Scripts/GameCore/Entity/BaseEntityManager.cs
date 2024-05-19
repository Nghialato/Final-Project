using Assets._Scripts.GameCore.Entity;
using System;
using UnityEngine;

namespace _Scripts.GameCore.Entity
{
    /// <summary>
    /// Hold the Data of Entity
    /// </summary>
    public abstract class BaseEntityManager : MonoBehaviour
    {
        public EntityState state;
        protected virtual void OnEnable()
        {
            var component = GetComponents<IComponentSystem>();
            for (int i = 0; i < component.Length; i++)
            {
                component[i].RegisterToSystem();
            }
        }

        private void OnDisable()
        {
            var component = GetComponents<IComponentSystem>();
            for (int i = 0; i < component.Length; i++)
            {
                component[i].RemoveFromSystem();
            }
        }

        public void EnableComponent(IComponentSystem componentSystem)
        {
            if (TryGetComponent(out componentSystem))
            {
                componentSystem.RegisterToSystem();
            } else Debug.LogError($"Entity does not contain {componentSystem}");
        }

        public void DisableComponent(IComponentSystem componentSystem)
        {
            if (TryGetComponent(out componentSystem))
            {
                componentSystem.RemoveFromSystem();
            } else Debug.LogError($"Entity does not contain {componentSystem}");
        }
    }
}