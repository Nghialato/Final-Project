using _Scripts.Algorithm;
using _Scripts.GameCore.Logic;
using UnityEngine;

namespace _Scripts.GameCore
{
    public class GameManager : MonoBehaviour
    {
        public GameSystem[] Systems;
        public FollowPlayer follower;
        private bool _playing;
        public EnemyLogic enemyLogic;
        public PlayerLogic player;

        private void Awake()
        {
            Systems = GetComponents<GameSystem>();
            for (int i = 0; i < Systems.Length; i++)
            {
                Systems[i].InitSystem();
            }
        }

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
            for (int i = 0; i < Systems.Length; i++)
            {
                Systems[i].RemoveAllFromSystem();
            }
        }

        private void Update()
        {
            if(!_playing) return;
            for (int i = 0; i < Systems.Length; i++)
            {
                Systems[i].UpdateSystem();
            }
        }
    }
}