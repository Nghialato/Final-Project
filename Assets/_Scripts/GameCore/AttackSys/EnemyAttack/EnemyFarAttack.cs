using Assets._Scripts.GameCore.AttackSys;
using System.Threading.Tasks;
using _Scripts.GameCore.Entity.Bullet;
using _Scripts.GameCore.MovementSys;
using UnityEngine;

namespace _Scripts.GameCore.AttackSys.EnemyAttack
{
    public class EnemyFarAttack : EntityAttack
    {
        public override void Attack(Vector3 startPosition, PositionData target)
        {
            if (_isCoolDown) return;
            CoolDownAttack();
            var bullet = Instantiate(bulletLogic);
            bullet.InitBullet(RootBullet.EnemyRoot, startPosition, target);
        }
        
        private async Task CoolDownAttack()
        {
            _isCoolDown = true;
            await Task.Delay((int)(1000 / maxAttackTimesPerSec));
            _isCoolDown = false;
        }
    }
}