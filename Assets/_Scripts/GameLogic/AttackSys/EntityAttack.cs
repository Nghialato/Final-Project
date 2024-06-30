using _Scripts.GameData;
using UnityEngine;

namespace _Scripts.GameLogic
{
    public abstract class EntityAttack : MonoBehaviour
    {
        public BulletLogic bulletLogic;
        public int maxAttackTimesPerSec;
        protected bool _isCoolDown;
        public abstract void Attack(Vector3 startPosition, PositionData target);
    }
}