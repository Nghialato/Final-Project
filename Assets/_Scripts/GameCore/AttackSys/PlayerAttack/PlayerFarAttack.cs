using System.Threading.Tasks;
using _Scripts.GameCore.Entity.Bullet;
using _Scripts.GameCore.MovementSys;
using Assets._Scripts.GameCore.AttackSys;
using UnityEngine;

namespace _Scripts.GameCore.AttackSys.PlayerAttack
{
    public class PlayerFarAttack : EntityAttack
    {
        public override void Attack(Vector3 startPosition, PositionData target)
        {
            if (_isCoolDown) return;
            CoolDownAttack();
            var bullet = Instantiate(bulletLogic);
            bullet.InitBullet(RootBullet.PlayerRoot, startPosition, target);
        }

        private async Task CoolDownAttack()
        {
            _isCoolDown = true;
            await Task.Delay((int)(1000 / maxAttackTimesPerSec));
            _isCoolDown = false;
        }
    }
}