using AYellowpaper.SerializedCollections;
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
        data = new SerializedDictionary<Vector2Int, int>();
    }

    public SerializedDictionary<Vector2Int, int> data;
}