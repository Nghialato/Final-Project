using UnityEngine;

namespace _Scripts.GameCore
{
    public class GameManager : MonoBehaviour
    {
        public GameSystem[] Systems;

        private void Awake()
        {
            Systems = GetComponents<GameSystem>();
            for (int i = 0; i < Systems.Length; i++)
            {
                Systems[i].InitSystem();
            }
        }

        private void Update()
        {
            for (int i = 0; i < Systems.Length; i++)
            {
                Systems[i].UpdateSystem();
            }
        }
    }
}