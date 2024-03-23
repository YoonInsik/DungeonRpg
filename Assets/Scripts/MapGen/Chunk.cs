using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;

    public void DrawChunk(ChunkData chunkData)
    {
        MapManager.Instance.tilemapVisualizer.PaintTiles(chunkData, floorTilemap);
    }
}
