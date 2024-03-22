using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private SerializedDictionary<Vector2Int, ChunkData> chunkDataDict;
    [SerializeField] private Transform chunkContainer;
    [SerializeField] private Chunk chunkPrefab;

    public TilemapVisualizer tilemapVisualizer;

    public const int CHUNKSIZE = 20;
    
    public List<Chunk> chunkList;

    public void Start()
    {
        foreach (var data in chunkDataDict)
        {
            RandomChunkData(data.Value);
        }
        SpawnChunkPlayerAround();
    }

    public void SpawnChunkPlayerAround()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                var newChunk = Instantiate(chunkPrefab, chunkContainer);
                newChunk.UpdateChunk(chunkDataDict[new Vector2Int(i, j)]);
                newChunk.transform.position = new Vector3(-CHUNKSIZE / 2, -CHUNKSIZE / 2, 0) + new Vector3(CHUNKSIZE * i, CHUNKSIZE * j);

                chunkList.Add(newChunk);
            }
        }
    }

    public void RandomChunkData(ChunkData _chunkData)
    {
        int a = Random.Range(1, 3);

        for (int i = 0; i < CHUNKSIZE; i++)
        {
            for (int j = 0; j < CHUNKSIZE; j++)
            {
                _chunkData.tileDatas[i, j] = a;
            }
        }
    }
}
