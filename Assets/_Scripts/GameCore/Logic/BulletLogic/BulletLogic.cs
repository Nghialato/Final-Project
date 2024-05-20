using System;
using _Scripts.GameCore.MovementSys;
using _Scripts.GameCore.ViewSys;
using UnityEngine;

namespace _Scripts.GameCore.Logic.BulletLogic
{
    public class BulletLogic : MonoBehaviour
    {
        public PositionData positionData;
        public ViewData viewData;
        private Vector3 _targetPosition;
        private bool _isFollowPlayer;

        public void UpdateTarget(Vector3 targetPosition, bool isFollowPlayer)
        {
            _targetPosition = targetPosition;
            _isFollowPlayer = isFollowPlayer;
        }

        #region Move Logic

        private void MoveToTarget()
        {
            var direction = _targetPosition - positionData.position;
            positionData.position += direction.normalized * (positionData.speed * Time.deltaTime);
            positionData.dirty = true;
        }        

        #endregion

        private void Update()
        {
            if (_isFollowPlayer)
            {
                if (Vector3.Distance(positionData.position, PlayerLogicEts.GetPosition()) >
                    viewData.viewRange)
                    UpdateTarget(PlayerLogicEts.GetPosition(), true);
                else _isFollowPlayer = false;
            }
            MoveToTarget();
        }
    }
}