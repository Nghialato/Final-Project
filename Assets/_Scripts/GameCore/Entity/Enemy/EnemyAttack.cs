using _Scripts.GameCore.AttackSys;
using UnityEngine;

namespace _Scripts.GameCore.Entity.Enemy
{
    public class EnemyAttack : AttackEntity
    {
        public override void Attack()
        {
            Debug.Log("Enemy Attack");
        }
    }
}