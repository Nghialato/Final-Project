using System;
using System.Collections.Generic;
using _Scripts.GameCore.Entity;
using _Scripts.GameCore.HealthSys;
using _Scripts.GameCore.MovementSys;
using _Scripts.GameCore.ViewSys;
using Assets._Scripts.GameCore.AttackSys;
using UnityEngine;

namespace _Scripts.GameCore.Logic
{
    public class PlayerLogic : MonoBehaviour
    {
        public HealthData healthData;
        public PositionData positionData;
        public ViewData viewData;
        public EntityAttack playerAttack;

        [SerializeField] private List<EnemyLogic> _enemyLogics = new(16);
        private EnemyLogic nearestEnemy;
        private bool isTargeting;
        private bool isPauseMoving;
        
        private void Awake()
        {
            this.Init();
        }

        #region Move Logic

        private Vector3 _directionMove;
        private Vector3 _directionNotMove;

        private void ModifyPosition()
        {
            if (_directionMove == Vector3.zero)
            {
                return;
            }
            positionData.dirty = true;
            positionData.position += _directionMove.normalized * (positionData.speedMove * Time.fixedDeltaTime);
            var targetRotation = 0f;
            if (isTargeting)
            {
                var directionTarget = nearestEnemy.positionData.position - positionData.position;
                targetRotation = Vector2.SignedAngle(Vector2.up, directionTarget);
            } else 
                targetRotation = Vector2.SignedAngle(Vector2.up, _directionMove);

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
        }

        private void FixedUpdate()
        {
            ModifyPosition();
            DetectNearestEnemy();
        }

        #endregion

        #region HealthLogic

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out EnemyLogic enemyManager))
            {
                enemyManager.DamageHealth(1);
                healthData.health -= 1;
                healthData.dirty = true;
            }

            if (other.gameObject.TryGetComponent(out IWall wall))
            {
                _directionNotMove = _directionMove;
                _directionMove = Vector3.zero;
                isPauseMoving = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IWall wall))
            {
                isPauseMoving = false;
                _directionNotMove = Vector3.zero;
            }
        }

        #endregion

        #region AttackLogic

        private void Attack()
        {
            if (nearestEnemy is null) return;
            playerAttack.Attack(positionData.position, nearestEnemy.positionData);
        }

        private void DetectNearestEnemy()
        {
            if (_enemyLogics.Count == 0)
            {
                nearestEnemy = null;
                isTargeting = false;
                return;
            }

            var distanceNearest = viewData.viewRange;
            for (int i = 0; i < _enemyLogics.Count; i++)
            {
                if (_enemyLogics[i].distanceToPlayer < distanceNearest)
                {
                    nearestEnemy = _enemyLogics[i];
                    isTargeting = true;
                }
            }
        }

        public EnemyLogic NearestEnemy() => nearestEnemy;

        public void EnemyDetector(EnemyLogic enemyLogic)
        {
            if (_enemyLogics.Contains(enemyLogic) == false) _enemyLogics.Add(enemyLogic);
        }

        public void EnemyLost(EnemyLogic enemyLogic)
        {
            if (_enemyLogics.Contains(enemyLogic)) _enemyLogics.Remove(enemyLogic);
        }

        #endregion
        
        private void Update()
        {
            if (Input.anyKey == false)
            {
                _directionMove = Vector3.zero;
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                if(_directionNotMove.x != 1)
                    _directionMove += Vector3.right;
            }
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                if(_directionNotMove.y != 1)
                    _directionMove += Vector3.up;
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                if(_directionNotMove.y != -1)
                    _directionMove += Vector3.down;
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                if(_directionNotMove.x != -1)
                    _directionMove += Vector3.left;
            }
            
            if (Input.GetKeyUp(KeyCode.A) && isPauseMoving == false)
            {
                _directionMove -= Vector3.left;
            }
            
            if (Input.GetKeyUp(KeyCode.D) && isPauseMoving == false)
            {
                _directionMove -= Vector3.right;
            }
            
            if (Input.GetKeyUp(KeyCode.W) && isPauseMoving == false)
            {
                _directionMove -= Vector3.up;
            }
            
            if (Input.GetKeyUp(KeyCode.S) && isPauseMoving == false)
            {
                _directionMove -= Vector3.down;
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) ||
                Input.GetKeyUp(KeyCode.W))
            {
                isPauseMoving = false;
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }

    }
}