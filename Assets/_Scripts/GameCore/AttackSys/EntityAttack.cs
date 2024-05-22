using _Scripts.GameCore.Logic;
using _Scripts.GameCore.MovementSys;
using UnityEngine;

namespace Assets._Scripts.GameCore.AttackSys
{
    public abstract class EntityAttack : MonoBehaviour
    {
        public BulletLogic bulletLogic;
        public int maxAttackTimesPerSec;
        protected bool _isCoolDown;
        public abstract void Attack(Vector3 startPosition, PositionData target);
    }
}