using _Scripts.GameData;
using UnityEngine;

namespace _Scripts.GameLogic
{
    public static class PlayerLogicEts
    {
        private static PlayerLogic _playerLogic;

        public static void Init(this PlayerLogic playerLogic)
        {
            _playerLogic = playerLogic;
        }

        public static Vector3 GetPosition() => _playerLogic.positionData.position;

        public static void SetPosition(Vector3 position)
        {
            _playerLogic.positionData.position = position;
            _playerLogic.positionData.dirty = true;
        }

        public static PositionData GetPositionData() => _playerLogic.positionData;

        public static bool CheckInRange(Vector3 enemyPosition)
        {
            return Vector3.Distance(enemyPosition, _playerLogic.positionData.position) <
                   _playerLogic.viewData.viewRange;
        }

        public static void DetectEnemy(EnemyLogic enemyLogic) => _playerLogic.EnemyDetector(enemyLogic);
        public static void LostEnemy(EnemyLogic enemyLogic) => _playerLogic.EnemyLost(enemyLogic);
    }
}