using System;
using UnityEngine;

namespace _Scripts.GameCore.AttackSys
{
    public abstract class AttackEntity : MonoBehaviour, IAttack
    {
        public void RegisterToSystem()
        {
            AttackSystemManagerEts.RegisterToArray(this);
        }

        public void RemoveFromSystem()
        {
            AttackSystemManagerEts.RemoveFromArray(this);
        }

        public void UpdateComponent()
        {
            Attack();
        }

        public abstract void Attack();

        private void OnDisable()
        {
            RemoveFromSystem();
        }
    }
}