using Assets._Scripts.GameCore.AttackSys;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace _Scripts.GameCore.AttackSys.EnemyAttack
{
    public class EnemyFarAttack : EntityAttack
    {

        public override void Attack(Vector3 startPosition, Vector3 target)
        {
            if (_isCoolDown) return;
            CoolDownAttack();
            var bullet = Instantiate(bulletLogic);
            bullet.positionData.position = startPosition;
            bullet.UpdateTarget(target);
        }
        
        private async Task CoolDownAttack()
        {
            _isCoolDown = true;
            await Task.Delay((int)(1000 / maxAttackTimesPerSec));
            _isCoolDown = false;
        }
    }
}