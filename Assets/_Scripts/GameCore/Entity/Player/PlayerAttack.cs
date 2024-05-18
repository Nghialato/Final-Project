using _Scripts.GameCore.AttackSys;
using UnityEngine;

namespace _Scripts.GameCore.Entity
{
    public class PlayerAttack : AttackEntity
    {
        public override void Attack()
        {
            Debug.Log("Player Attack");
        }
    }
}