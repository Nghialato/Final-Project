using System;
using UnityEngine;

namespace _Scripts.GameCore.MovementSys
{
    public abstract class MovementEntity : MonoBehaviour, IMovement
    {
        public abstract void Move();

        public void RegisterToSystem()
        {
            MovementSystemManagerEts.RegisterToArray(this);
        }

        public void RemoveFromSystem()
        {
            MovementSystemManagerEts.RemoveFromArray(this);
        }

        public void UpdateComponent()
        {
            Move();
        }

        private void OnDisable()
        {
            RemoveFromSystem();
        }
    }
}