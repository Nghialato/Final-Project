namespace _Scripts.GameCore.AttackSys
{
    public static class AttackSystemManagerEts
    {
        private static AttackSystemManager _attackSystemManager;

        public static void Init(this AttackSystemManager attackSystemManager)
        {
            _attackSystemManager = attackSystemManager;
        }

        public static void RegisterToArray(IAttack attack)
        {
            _attackSystemManager.RegisterToSystem(attack);
        }

        public static void RemoveFromArray(IAttack attack)
        {
            _attackSystemManager.RemoveFromSystem(attack);
        }
    }
}