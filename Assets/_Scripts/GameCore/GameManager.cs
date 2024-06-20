using _Scripts.Algorithm;
using _Scripts.GameCore.Logic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.GameCore
{
    public class GameManager : MonoBehaviour
    {
        public GameSystem[] systems;
        
        private void Awake()
        {
            systems = GetComponents<GameSystem>();
            foreach (var system in systems)
            {
                system.InitSystem();
            }
        }
        
        private void Update()
        {
            if(!_playing) return;
            foreach (var system in systems)
            {
                system.UpdateSystem();
            }
        }

        #region Core Game

        public FollowPlayer follower;
        private bool _playing;
        public EnemyLogic enemyLogic;
        public PlayerLogic player;
        
        public void PlayGame()
        {
            Instantiate(player);
            _playing = true;
            var position = DungeonGenerator.Instance.GetRandomPositionForPlayer();
            var playerPos = new Vector3(position.x, position.y, 0);
            PlayerLogicEts.SetPosition(playerPos);
            follower.enabled = true;
            Camera.main.orthographicSize = 8;

            var listRoom = DungeonGenerator.Instance.GetListRoom();

            foreach (var room in listRoom)
            {
                if(DungeonGenerator.Instance.IsPlayerRoom(room.roomId)) continue;
                Instantiate(enemyLogic);
                enemyLogic.Init(room.GetCenter(), room.GetRandomPointInside(), room.GetRandomPointInside());
            }

        }

        public void PauseGame()
        {
            _playing = false;
            follower.ReturnStartPoint();
            follower.enabled = false;
            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].RemoveAllFromSystem();
            }
        }

        #endregion
    }
}