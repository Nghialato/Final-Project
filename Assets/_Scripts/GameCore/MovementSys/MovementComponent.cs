using System;
using UnityEngine;

namespace _Scripts.GameCore.MovementSys
{
    public abstract class MovementComponent : BaseComponent<PositionData, MovementSystemManager>, IMovement
    {
        protected Transform _transform;
        public abstract void MoveUpdate();

        public override void RegisterToSystem()
        {
            base.RegisterToSystem();
            _transform = transform;
        }

        public override void UpdateComponent()
        {
            if (ComponentData.dirty == false) return;
            MoveUpdate();
            ComponentData.dirty = false;
        }

        private void OnDisable()
        {
            RemoveFromSystem();
        }
    }
}