using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{

    //need?

    [SerializeField]
    protected TilemapVisualiser tilemapVisualiser = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateMap()
    {
        tilemapVisualiser.Clear();
        runProceduralGeneration();
    }

    protected abstract void runProceduralGeneration();
}
