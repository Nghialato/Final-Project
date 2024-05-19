﻿using System;
using UnityEngine;

namespace _Scripts.GameCore.MovementSys
{
    public abstract class MovementEntity : MonoBehaviour, IMovement
    {
        public PositionData positionData;
        protected Transform _transform;
        public abstract void Move();

        public void RegisterToSystem()
        {
            _transform = transform;
            MovementSystemManagerEts.RegisterToArray(this);
        }

        public void RemoveFromSystem()
        {
            MovementSystemManagerEts.RemoveFromArray(this);
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