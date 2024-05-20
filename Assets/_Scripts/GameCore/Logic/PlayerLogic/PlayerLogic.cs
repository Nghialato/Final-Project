using System.Collections.Generic;
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
        
        private void Awake()
        {
            this.Init();
        }

        #region Move Logic

        private Vector3 _directionMove;

        private void ModifyPosition()
        {
            if (_directionMove == Vector3.zero)
            {
                return;
            }
            positionData.dirty = true;
            positionData.position += _directionMove.normalized * (positionData.speed * Time.deltaTime);
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
        }

        #endregion

        #region AttackLogic

        private void Attack()
        {
            if (nearestEnemy is null) return;
            playerAttack.Attack(positionData.position, nearestEnemy.positionData.position);
        }

        private void DetectNearestEnemy()
        {
            if (_enemyLogics.Count == 0)
            {
                nearestEnemy = null;
                return;
            }

            var distanceNearest = viewData.viewRange;
            for (int i = 0; i < _enemyLogics.Count; i++)
            {
                if (_enemyLogics[i].distanceToPlayer < distanceNearest)
                {
                    nearestEnemy = _enemyLogics[i];
                }
            }
        }

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
            if (Input.GetKeyDown(KeyCode.A))
            {
                _directionMove += Vector3.left;
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                _directionMove += Vector3.right;
            }
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                _directionMove += Vector3.up;
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                _directionMove += Vector3.down;
            }
            
            if (Input.GetKeyUp(KeyCode.A))
            {
                _directionMove -= Vector3.left;
            }
            
            if (Input.GetKeyUp(KeyCode.D))
            {
                _directionMove -= Vector3.right;
            }
            
            if (Input.GetKeyUp(KeyCode.W))
            {
                _directionMove -= Vector3.up;
            }
            
            if (Input.GetKeyUp(KeyCode.S))
            {
                _directionMove -= Vector3.down;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }

    }
}