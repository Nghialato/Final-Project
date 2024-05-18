using _Scripts.GameCore.Entity;

namespace _Scripts.GameCore
{
    public interface IComponentSystem
    {
        void RegisterToSystem();
        void RemoveFromSystem();
        void UpdateComponent();
    }
}