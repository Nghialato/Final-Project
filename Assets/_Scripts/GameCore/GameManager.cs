using System;
using System.Collections.Generic;
using _Scripts.GameCore.HealthSys;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.GameCore
{
    public class GameManager : MonoBehaviour
    {
        public List<GameSystem> Systems = new(4);

        private void Awake()
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                Systems[i].InitSystem();
            }
        }

        private void Update()
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                Systems[i].UpdateSystem();
            }
        }
    }
}