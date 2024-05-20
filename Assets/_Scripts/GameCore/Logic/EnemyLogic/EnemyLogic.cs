using _Scripts.GameCore.HealthSys;
using _Scripts.GameCore.MovementSys;
using _Scripts.GameCore.ViewSys;
using Assets._Scripts.GameCore.AttackSys;
using UnityEngine;

namespace _Scripts.GameCore.Logic
{
    public class EnemyLogic : MonoBehaviour
    {
        public HealthData healthData;
        public PositionData positionData;
        public ViewData viewData;
        public EntityAttack enemyAttack;
        public float distanceToPlayer;

        private bool _isDetected;

        public Vector3[] pathMove;

        private void Awake() 
        {
            pathMoveLength = Vector3.Distance(pathMove[0], pathMove[1]);
            positionData.position = pathMove[0];
            positionData.dirty = true;
        }

        #region Health Logic

        public void DamageHealth(int dame)
        {
            healthData.health -= dame;
            healthData.dirty = true;
        }

        #endregion

        #region Move Logic

        private enum MoveType
        {
            MoveToPlayer, MoveBySetPath
        }

        private MoveType moveType;
        private float processMoveByPath;
        private bool isChangeStateMove;
        private int moveReserve = 1;
        private float pathMoveLength;

        private void MoveToPlayer()
        {
            var direction = PlayerLogicEts.GetPosition() - positionData.position;
            positionData.position += direction.normalized * (positionData.speed * Time.deltaTime);
            positionData.dirty = true;
        }

        private void MoveBySetPath()
        {
            var direction = (pathMove[1] - pathMove[0]) * moveReserve;
            positionData.position += direction.normalized * (positionData.speed * Time.deltaTime);
            processMoveByPath = Mathf.Clamp(Vector3.Distance(positionData.position, moveReserve == 1 ? pathMove[0] : pathMove[1]) / pathMoveLength, 0, 1);
            if (processMoveByPath == 1)
            {
                positionData.position = moveReserve == 1 ? pathMove[1] : pathMove[0];
                moveReserve *= -1;
                processMoveByPath = 0;
            }
            positionData.dirty = true;
        }

        private void Move()
        {
            if (isChangeStateMove && moveType == MoveType.MoveBySetPath)
            {
                var positionMoveTo = (pathMove[1] - pathMove[0]) * processMoveByPath + pathMove[0];
                if (Vector3.Distance(positionMoveTo, positionData.position) < .1f)
                {
                    positionData.position = positionMoveTo;
                    positionData.dirty = true;
                    isChangeStateMove = false;
                    return;
                }
                var directionMove = positionMoveTo - positionData.position;
                positionData.position += directionMove.normalized * (positionData.speed * Time.deltaTime);
                positionData.dirty = true;
                return;
            }
            switch (moveType)
            {
                case MoveType.MoveToPlayer:
                    MoveToPlayer();
                    break;
                case MoveType.MoveBySetPath:
                     MoveBySetPath();
                    break;
            }
        }

        private void CheckInPlayerRangeView()
        {
            var inRange = PlayerLogicEts.CheckInRange(positionData.position);
            if(inRange && _isDetected == false)
            {
                PlayerLogicEts.DetectEnemy(this);
                distanceToPlayer = Vector3.Distance(PlayerLogicEts.GetPosition(), positionData.position);
                _isDetected = true;
                return;
            }

            if (inRange || !_isDetected) return;
            PlayerLogicEts.LostEnemy(this);
            _isDetected = false;
        }

        #endregion

        private void Update()
        {
            if(Vector3.Distance(PlayerLogicEts.GetPosition(), positionData.position) < viewData.viewRange)
            {
                if (moveType != MoveType.MoveToPlayer)
                {
                    isChangeStateMove = true;
                    moveType = MoveType.MoveToPlayer;
                    isChangeStateMove = false;
                }
            }
            else
            {
                if (moveType != MoveType.MoveBySetPath)
                {
                    isChangeStateMove = true;
                    moveType = MoveType.MoveBySetPath;
                }
            }
            Move();
            CheckInPlayerRangeView();
        }

        private void OnDisable()
        {
            PlayerLogicEts.LostEnemy(this);
        }
    }
}