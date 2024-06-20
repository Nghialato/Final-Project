using Assets._Scripts.GameCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.GameCore.Entity
{
    /// <summary>
    /// Hold the Data of Entity
    /// </summary>
    public abstract class BaseEntityManager : MonoBehaviour
    {
        public EntityState state;
        private List<IComponent> listComponent;
        protected virtual void OnEnable()
        {
            listComponent = GetComponents<IComponent>().ToList();
            foreach (var component in listComponent)
            {
                component.RegisterToSystem();
            }
        }

        private void OnDisable()
        {
            foreach (var component in listComponent)
            {
                component.RemoveFromSystem();
            }
        }

        public void EnableComponent(IComponent component)
        {
            if (TryGetComponent(out component))
            {
                component.RegisterToSystem();
            } else Debug.LogError($"Entity does not contain {component}");
        }

        public void DisableComponent(IComponent component)
        {
            if (TryGetComponent(out component))
            {
                component.RemoveFromSystem();
            } else Debug.LogError($"Entity does not contain {component}");
        }
    }
}