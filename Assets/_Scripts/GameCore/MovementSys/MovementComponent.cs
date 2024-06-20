using System;
using UnityEngine;

namespace _Scripts.GameCore.MovementSys
{
    public abstract class MovementComponent : MonoBehaviour, IMovement
    {
        public PositionData positionData;
        protected Transform _transform;
        public abstract void Move();

        public void RegisterToSystem()
        {
            _transform = transform;
            MovementSystemManagerEts.RegisterToArray(this);
        }

        public virtual void RemoveFromSystem()
        {
            MovementSystemManagerEts.RemoveFromArray(this);
            gameObject.SetActive(false);
        }

        public void UpdateComponent()
        {
            if (positionData.dirty == false) return;
            Move();
            positionData.dirty = false;
        }

        private void OnDisable()
        {
            RemoveFromSystem();
        }
    }
}