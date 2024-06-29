using UnityEngine;

namespace _Scripts.GameCore
{
    public abstract class BaseComponent<T, TK> : MonoBehaviour, IComponent where T : ComponentData where TK : GameSystem
    {
        protected T ComponentData;
        private TK _gameSystem;

        public void Init()
        {
            ComponentData = GetComponent<T>();
            _gameSystem = (TK)GameManager.listSystem[typeof(TK)];
        }

        public virtual void RegisterToSystem()
        {
            _gameSystem.RegisterToSystem(this);
        }

        public virtual void RemoveFromSystem()
        {
                _gameSystem.RemoveFromSystem(this);
        }

        public virtual void UpdateComponent()
        {
        }

        private void OnDisable()
        {
            RemoveFromSystem();
        }
    }
}