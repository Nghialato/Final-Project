using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField] protected int timeCheck;

    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        var watch = new System.Diagnostics.Stopwatch();
            
        watch.Start();
        for (int i = 0; i < timeCheck; i++)
        {
            RunProceduralGeneration();
        }
        watch.Stop();
        Debug.Log(watch.ElapsedMilliseconds);
    }

    protected abstract void RunProceduralGeneration();
}
