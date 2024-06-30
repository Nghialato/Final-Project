using _Scripts.GameCore.Entity;
using _Scripts.GameLogic;
using _Scripts.GameData;
using UnityEngine;

public class FollowPlayer : BaseEntityManager
{
    public PositionData m_position;
    private Vector3 startPos;
    private float startSize;

    private void Start()
    {
        startPos = transform.position;
        startSize = 75;
    }

    public void ReturnStartPoint()
    {
        transform.position = startPos;
        GetComponent<Camera>().orthographicSize = startSize;
    }

    private void FixedUpdate()
    {
        m_position.position = PlayerLogicEts.GetPosition();
        m_position.dirty = true;
    }
}
