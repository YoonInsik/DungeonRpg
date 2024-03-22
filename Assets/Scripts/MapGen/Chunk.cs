using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    [SerializeField] private ChunkData chunkData;
    [SerializeField] private Tilemap floorTilemap;

    public void UpdateChunk(ChunkData _chunkData)
    {
        SetChunkData(_chunkData);
        DrawChunk();
    }

    private void SetChunkData(ChunkData _chunkData)
    {
        chunkData = _chunkData;
    }

    private void DrawChunk()
    {
        MapManager.Instance.tilemapVisualizer.PaintTiles(chunkData, floorTilemap);
    }
}
