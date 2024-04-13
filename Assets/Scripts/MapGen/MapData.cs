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

    public string StringChunkType 
    { 
        get 
        {
            string value;
            switch (chunkType)
            {
                case ChunkType.Start:
                    value = "시작";
                    break;
                case ChunkType.Boss:
                    value = "보스";
                    break;
                case ChunkType.Normal:
                    value = "일반";
                    break;
                case ChunkType.Many:
                    value = "더 많은 적";
                    break;
                default:
                    value = "오류 방";
                    break;
            }
            return value;
        }
    }
}

public enum ChunkType
{
    Start,
    Boss,
    Normal,
    Many,
}