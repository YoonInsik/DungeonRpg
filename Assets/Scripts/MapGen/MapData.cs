using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "SO/MapData")]
public class MapData : ScriptableObject
{
    public MapData()
    {
        data = new SerializedDictionary<Vector2Int, ChunkData>();
    }

    public SerializedDictionary<Vector2Int, ChunkData> data;
}

[System.Serializable]
public class ChunkData
{
    public ChunkData()
    {
        dropTable = new List<DropData>();
    }

    public List<DropData> dropTable;
    public ChunkType chunkType;
}

public enum ChunkType
{
    Start,
    Boss,
    Normal,
    Many,
}