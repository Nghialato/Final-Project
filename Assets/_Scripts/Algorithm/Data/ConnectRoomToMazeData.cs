using UnityEngine;

namespace _Scripts.Algorithm.Data
{
    [CreateAssetMenu(fileName ="ConnectRoomToMazeData",menuName = "PCG/ConnectRoomToMazeData")]
    public class ConnectRoomToMazeData : ScriptableObject
    {
        [Range(0, 100)] 
        public int chanceToChangePortal;
        
        [Range(0, 100)]
        public int imperfectRate;
    }
}