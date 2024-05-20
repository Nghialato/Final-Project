namespace _Scripts.GameCore.ViewSys
{
    public static class ViewSystemManagerEts
    {
        private static ViewSystemManager _viewSystemManager;

        public static void Init(this ViewSystemManager viewSystemManager)
        {
            _viewSystemManager = viewSystemManager;
        }
        
        public static void RegisterToArray(IView movement)
        {
            _viewSystemManager.RegisterToSystem(movement);
        }

        public static void RemoveFromArray(IView movement)
        {
            _viewSystemManager.RemoveFromSystem(movement);
        }
    }
}