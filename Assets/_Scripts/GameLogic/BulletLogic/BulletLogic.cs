using _Scripts.GameCore.Entity.Bullet;
using _Scripts.GameData;
using UnityEngine;

namespace _Scripts.GameLogic
{
    public class BulletLogic : MonoBehaviour
    {
        public PositionData positionData;
        public ViewData viewData;
        public RootBullet rootBullet;
        [SerializeField] private PositionData targetPosition;
        private Vector3 realTarget;
        [SerializeField] private bool isFollowTarget;
        private Vector3 direction;

        public void InitBullet(RootBullet rootBulletInit, Vector3 positionInit, PositionData target, bool setFollowTarget = false)
        {
            rootBullet = rootBulletInit;
            positionData.position = positionInit;
            realTarget = target.position;
            positionData.dirty = true;
            targetPosition = target;
            isFollowTarget = setFollowTarget;
            direction = realTarget - positionData.position;
        }

        #region Move Logic

        private void MoveToTarget()
        {
            positionData.position += direction.normalized * (positionData.speedMove * Time.deltaTime);
            positionData.dirty = true;
        }

        private bool IsTargetOutRange()
        {
            return Vector3.Distance(positionData.position, targetPosition.position) >
                   viewData.viewRange;
        }

        #endregion

        private void Update()
        {
            if (isFollowTarget)
            {
                if (IsTargetOutRange())
                {
                    realTarget = targetPosition.position;
                    direction = realTarget - positionData.position;
                }
                else isFollowTarget = false;
            }
            MoveToTarget();
        }
    }
}