using System;
using _Scripts.GameCore.Entity.Enemy;
using _Scripts.GameCore.HealthSys;
using _Scripts.GameCore.MovementSys;
using Assets._Scripts.GameCore.AttackSys;
using UnityEngine;

namespace _Scripts.GameCore.Logic
{
    public class PlayerLogic : MonoBehaviour
    {
        public HealthData healthData;
        public PositionData positionData;
        public EntityAttack playerAttack;

        public static PlayerLogic instance;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        #region Move Logic

        private Vector3 _directionMove;

        public Vector3 GetPlayerPosition() => positionData.position;
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
        }

        #endregion

        #region HealthLogic

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Collision");
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
            playerAttack.Attack();
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