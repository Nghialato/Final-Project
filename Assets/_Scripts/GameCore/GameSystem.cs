using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.GameCore
{
    public abstract class GameSystem : MonoBehaviour
    {
        private List<IComponentSystem> ComponentSystems = new(32);
        public abstract void InitSystem();

        public void RegisterToSystem(IComponentSystem component)
        {
            ComponentSystems.Add(component);
        }

        public void RemoveFromSystem(IComponentSystem component)
        {
            ComponentSystems.Remove(component);
        }

        public void RemoveAllFromSystem()
        {
            for (var index = 0; index < ComponentSystems.Count; index++)
            {
                ComponentSystems[index].RemoveFromSystem();
            }
        }

        public void UpdateSystem()
        {
            for (int i = 0; i < ComponentSystems.Count; i++)
            {
                ComponentSystems[i].UpdateComponent();
            }
        }
    }
}