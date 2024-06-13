using _Scripts.GameCore.Entity.Bullet;
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

        public void Init(Vector2Int startPos, Vector2Int pathStart, Vector2Int pathEnd)
        {
            positionData.position = new Vector3(startPos.x, startPos.y);
            positionData.dirty = true;
            pathMove[0] = new Vector3(pathStart.x, pathStart.y);
            pathMove[1] = new Vector3(pathEnd.x, pathEnd.y);
            gameObject.SetActive(true);
        }

        #region Health Logic

        public void DamageHealth(int dame)
        {
            healthData.health -= dame;
            healthData.dirty = true;
            CheckAlive();
        }

        private void CheckAlive()
        {
            if (healthData.health > 0) return;
            Death();
        }

        private void Death()
        {
            gameObject.SetActive(false);
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
            positionData.position += direction.normalized * (positionData.speedMove * Time.deltaTime);
            var targetRotation = Vector2.SignedAngle(Vector2.up, direction);
            if (targetRotation > positionData.rotation)
            {
                positionData.rotation += positionData.speedRotate;
                if (positionData.rotation > targetRotation) positionData.rotation = targetRotation;
            }
            else
            {
                positionData.rotation -= positionData.speedRotate;
                if (positionData.rotation < targetRotation) positionData.rotation = targetRotation;
            }
            positionData.dirty = true;
        }

        private void MoveBySetPath()
        {
            var direction = (pathMove[1] - pathMove[0]) * moveReserve;
            positionData.position += direction.normalized * (positionData.speedMove * Time.deltaTime);
            processMoveByPath = Mathf.Clamp(Vector3.Distance(positionData.position, moveReserve == 1 ? pathMove[0] : pathMove[1]) / pathMoveLength, 0, 1);
            if (processMoveByPath == 1)
            {
                positionData.position = moveReserve == 1 ? pathMove[1] : pathMove[0];
                moveReserve *= -1;
                processMoveByPath = 0;
            }
            var targetRotation = Vector2.SignedAngle(Vector2.up, direction);
            if (targetRotation > positionData.rotation)
            {
                positionData.rotation += positionData.speedRotate;
                if (positionData.rotation > targetRotation) positionData.rotation = targetRotation;
            }
            else
            {
                positionData.rotation -= positionData.speedRotate;
                if (positionData.rotation < targetRotation) positionData.rotation = targetRotation;
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
                positionData.position += directionMove.normalized * (positionData.speedMove * Time.deltaTime);
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

        #region Attack Logic

        private void TryAttack()
        {
            enemyAttack.Attack(positionData.position, PlayerLogicEts.GetPositionData());
        }

        #endregion

        #region View Logic

        private bool IsSawPlayer()
        {
            return Vector3.Distance(PlayerLogicEts.GetPosition(), positionData.position) < viewData.viewRange;
        }

        #endregion
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.TryGetComponent(out BulletLogic bulletLogic))
            {
                if (bulletLogic.rootBullet == RootBullet.PlayerRoot)
                {
                    DamageHealth(1);
                }
            }
        }

        private void Update()
        {
            if(IsSawPlayer())
            {
                if (moveType != MoveType.MoveToPlayer)
                {
                    isChangeStateMove = true;
                    moveType = MoveType.MoveToPlayer;
                    isChangeStateMove = false;
                }
                TryAttack();
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