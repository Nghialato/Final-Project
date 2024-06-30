using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.GameCore
{
    public abstract class GameSystem : MonoBehaviour
    {
        public List<IComponent> ComponentSystems = new(32);
        public abstract void InitSystem();

        public void RegisterToSystem(IComponent component)
        {
            ComponentSystems.Add(component);
        }

        public void RemoveFromSystem(IComponent component)
        {
            ComponentSystems.Remove(component);
        }

        public void RemoveAllFromSystem()
        {
            for (var index = 0; index < ComponentSystems.Count; index++)
            {
                RemoveFromSystem(ComponentSystems[index]);
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