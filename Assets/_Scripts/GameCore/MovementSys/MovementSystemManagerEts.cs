namespace _Scripts.GameCore.MovementSys
{
    public static class MovementSystemManagerEts
    {
        private static MovementSystemManager _movementSystemManager;

        public static void Init(this MovementSystemManager movementSystemManager)
        {
            _movementSystemManager = movementSystemManager;
        }

        public static void RegisterToArray(IMovement movement)
        {
            _movementSystemManager.RegisterToSystem(movement);
        }

        public static void RemoveFromArray(IMovement movement)
        {
            _movementSystemManager.RemoveFromSystem(movement);
        }
    }
}