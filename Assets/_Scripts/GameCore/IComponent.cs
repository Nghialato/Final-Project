namespace _Scripts.GameCore
{
    public interface IComponent
    {
        void RegisterToSystem();
        void RemoveFromSystem();
        void UpdateComponent();
    }
}