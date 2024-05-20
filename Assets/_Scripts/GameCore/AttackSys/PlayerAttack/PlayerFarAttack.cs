using System.Threading.Tasks;
using _Scripts.GameCore.Entity;
using _Scripts.GameCore.Entity.Bullet;
using Assets._Scripts.GameCore.AttackSys;
using UnityEngine;

namespace _Scripts.GameCore.AttackSys.PlayerAttack
{
    public class PlayerFarAttack : EntityAttack
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