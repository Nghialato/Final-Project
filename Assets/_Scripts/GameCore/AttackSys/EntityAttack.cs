using UnityEngine;

namespace Assets._Scripts.GameCore.AttackSys
{
    public abstract class EntityAttack : MonoBehaviour
    {
        public float rangeView;
        public abstract void Attack();
    }
}