namespace _Scripts.GameCore
{
    public interface IComponent
    {
        void Init();
        void RegisterToSystem();
        void RemoveFromSystem();
        void UpdateComponent();
    }
}