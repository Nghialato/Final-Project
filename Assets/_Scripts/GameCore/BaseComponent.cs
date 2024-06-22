using UnityEngine;

namespace _Scripts.GameCore
{
    public abstract class BaseComponent<T, TK> : MonoBehaviour, IComponent where T : ComponentData where TK : GameSystem
    {
        protected T ComponentData;
        private TK _gameSystem;

        private void Awake()
        {
            ComponentData = GetComponent<T>();
            _gameSystem = FindObjectOfType<TK>();
        }

        public virtual void RegisterToSystem()
        {
            if (_gameSystem == null) _gameSystem = FindObjectOfType<TK>();
            _gameSystem.RegisterToSystem(this);
        }

        public virtual void RemoveFromSystem()
        {
            _gameSystem.RegisterToSystem(this);
        }

        public virtual void UpdateComponent()
        {
        }
    }
}