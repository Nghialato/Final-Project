namespace _Scripts.GameCore
{
    public interface IComponentSystem
    {
        void RegisterToSystem();
        void RemoveFromSystem();
        void UpdateComponent();
    }
}