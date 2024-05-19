using _Scripts.GameCore.HealthSys;
using _Scripts.GameCore.MovementSys;
using Assets._Scripts.GameCore.AttackSys;
using UnityEngine;

namespace _Scripts.GameCore.Logic
{
    public class EnemyLogic : MonoBehaviour
    {
        public HealthData healthData;
        public PositionData positionData;
        public EntityAttack enemyAttack;

        public Vector3[] pathMove;

        #region Health Logic

        public void DamageHealth(int dame)
        {
            healthData.health -= dame;
            healthData.dirty = true;
        }

        #endregion

        #region Move Logic

        internal enum MoveType
        {
            MoveToPlayer, MoveBySettedPath
        }

        private MoveType moveType;
        private float processMoveByPath;
        private bool isChangeStateMove;

        private void MoveToPlayer()
        {

        }

        private void MoveBySettedPath()
        {

        }

        public void Move()
        {
            switch (moveType)
            {
                case MoveType.MoveToPlayer:
                    MoveToPlayer();
                    break;
                case MoveType.MoveBySettedPath:
                     MoveBySettedPath();
                    break;
            }
        }

        public void CheckInRangeView(Vector3 distance)
        {

        }

        #endregion

        private void Update()
        {
            if(Vector3.Distance(PlayerLogic.instance.GetPlayerPosition(), positionData.position) < enemyAttack.rangeView)
            {

            }
            Move();
        }

    }
}