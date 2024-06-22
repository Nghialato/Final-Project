using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.GameCore.MovementSys
{
    public class PositionData : ComponentData
    {
        public Vector3 position;
        public float rotation;
        public float speedMove;
        public float speedRotate;
    }
}