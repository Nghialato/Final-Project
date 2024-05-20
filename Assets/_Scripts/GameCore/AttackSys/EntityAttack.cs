using _Scripts.GameCore.Logic.BulletLogic;
using UnityEngine;

namespace Assets._Scripts.GameCore.AttackSys
{
    public abstract class EntityAttack : MonoBehaviour
    {
        public BulletLogic bulletLogic;
        public int maxAttackTimesPerSec;
        protected bool _isCoolDown;
        public abstract void Attack(Vector3 startPosition, Vector3 target);
    }
}