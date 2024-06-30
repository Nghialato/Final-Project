using _Scripts.GameLogic;
using UnityEngine;

namespace _Scripts.GameCore.Entity
{
    public class WallEntity : MonoBehaviour, IWall
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out BulletLogic bulletLogic))
            {
                bulletLogic.gameObject.SetActive(false);
            }
        }
    }
}