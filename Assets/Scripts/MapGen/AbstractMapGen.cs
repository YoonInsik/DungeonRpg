using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMapGen : MonoBehaviour
{
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDungeon(out HashSet<Vector2Int> floorPositions, out HashSet<Vector2Int> wallPositions)
    {
        RunProceduralGeneration(out floorPositions, out wallPositions);
    }

    protected abstract void RunProceduralGeneration(out HashSet<Vector2Int> floorPositions, out HashSet<Vector2Int> wallPositions);
}
