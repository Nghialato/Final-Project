using UnityEngine;

namespace _Scripts.GameCore.Entity
{
    public abstract class BaseEntity : MonoBehaviour
    {
        private void Awake()
        {
            var component = GetComponents<IComponentSystem>();
            for (int i = 0; i < component.Length; i++)
            {
                component[i].RegisterToSystem();
            }
        }
    }
}