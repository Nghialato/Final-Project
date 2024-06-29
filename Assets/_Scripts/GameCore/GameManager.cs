using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _Scripts.Algorithm;
using _Scripts.GameCore.Logic;
using _Scripts.GameCore.MovementSys;
using Unity.VisualScripting;
using UnityEngine;
namespace _Scripts.GameCore
{
    public class GameManager : MonoBehaviour
    {
        public static Dictionary<Type, Component> listSystem = new(4);
        public GameSystem[] systems;
        
        private void Awake()
        {
            systems = GetComponents<GameSystem>();
            
            var pType = typeof(GameSystem);
            var children = Enumerable.Range(1, 1).SelectMany(i => Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.IsClass && t != pType
                                          && pType.IsAssignableFrom(t))
                    .Select(t => t));
            
            foreach (var child in children)
            {
                listSystem.Add(child, FindObjectOfType(child).GetComponent(child));
            }
        }
        
        private void Update()
        {
            if(!_playing) return;
            foreach (var system in systems)
            {
                if(system is MovementSystemManager) continue;
                system.UpdateSystem();
            }
        }

        private void FixedUpdate()
        {
            foreach (var system in systems)
            {
                if (system is MovementSystemManager)
                {
                    system.UpdateSystem();
                    break;
                }
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