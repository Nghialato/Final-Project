namespace _Scripts.GameCore.HealthSys
{
    public static class HealthSystemManagerEts
    {
        private static HealthSystemManager _healthSystemManager;

        public static void Init(this HealthSystemManager healthSystemManager)
        {
            _healthSystemManager = healthSystemManager;
        }
        
        public static void RegisterToArray(in IHealth health)
        {
            _healthSystemManager.RegisterToSystem(health);
        }

        public static void RemoveFromArray(in IHealth health)
        {
            _healthSystemManager.RemoveFromSystem(health);
        }
    }
}