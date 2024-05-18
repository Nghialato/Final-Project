using _Scripts.GameCore.HealthSys;
using _Scripts.GameCore.MovementSys;
using UnityEngine;

namespace _Scripts.GameCore.Logic
{
    public class EnemyLogic : MonoBehaviour
    {
        public HealthData healthData;
        public PositionData positionData;

        #region Health Logic

        public void DamageHealth(int dame)
        {
            healthData.health -= dame;
            healthData.dirty = true;
        }

        #endregion
        
    }
}