using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private MapData mapData;
    [SerializeField] private Transform chunkContainer;
    [SerializeField] private Chunk chunkPrefab;

    public TilemapVisualizer tilemapVisualizer;
    public Vector2Int beforeChunk;

    public const int CHUNKSIZE = 20;
    public const int HALFCHUNKSIZE = 10;
    
    public List<Chunk> chunkList;

    public void Start()
    {
        SetChunkData(Vector2Int.zero);
        SpawnChunkPlayerAround();
    }

    public void SetChunkData(Vector2Int center)
    {
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                mapData.data.TryAdd(center + new Vector2Int(i, j), RandomChunkData());
            }
        }
    }

    public void SpawnChunkPlayerAround()
    {
        beforeChunk = Vector2Int.zero;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                var newChunk = Instantiate(chunkPrefab, chunkContainer);
                newChunk.name = $"Chunk {i} {j}";
                newChunk.DrawChunk(mapData.data[new Vector2Int(i, j)]);
                newChunk.transform.position = new Vector3(CHUNKSIZE * i, CHUNKSIZE * j);

                chunkList.Add(newChunk);
            }
        }
    }

    public ChunkData RandomChunkData()
    {
        ChunkData newChunkData = new ChunkData();

        for (int i = 0; i < CHUNKSIZE; i++)
        {
            for (int j = 0; j < CHUNKSIZE; j++)
            {
                newChunkData.data.Add(new Vector2Int(i, j), Random.Range(1, 3));
            }
        }

        return newChunkData;
    }

    public void ReloadChunks()
    {
        Vector2 playerPos = UnitManager.Instance.player.transform.position;

        Vector2Int playerChunkPos = new Vector2Int(
            Mathf.RoundToInt(playerPos.x / CHUNKSIZE),
            Mathf.RoundToInt(playerPos.y / CHUNKSIZE));

        SetChunkData(playerChunkPos);

        Vector2Int dir = playerChunkPos - beforeChunk;
        foreach (var moveChunk in chunkList)
        {
            Vector2Int moveVec = new Vector2Int(0, 0);
            if (Mathf.Abs(dir.x) > 0)
            {
                var moveChunkVec = moveChunk.transform.position.x - beforeChunk.x * CHUNKSIZE;
                if (moveChunkVec != 0 && Mathf.Sign(moveChunkVec) != Mathf.Sign(dir.x))
                {
                    moveVec += dir;
                }
            }
            if (Mathf.Abs(dir.y) > 0)
            {
                var moveChunkVec = moveChunk.transform.position.y - beforeChunk.y * CHUNKSIZE;
                if (moveChunkVec != 0 && Mathf.Sign(moveChunkVec) != Mathf.Sign(dir.y))
                {
                    moveVec += dir;
                }
            }

            moveChunk.transform.Translate(new Vector2(moveVec.x, moveVec.y) * CHUNKSIZE * 3);

            Vector2Int moveChunkPos = new Vector2Int(
            Mathf.RoundToInt(moveChunk.transform.position.x / CHUNKSIZE),
            Mathf.RoundToInt(moveChunk.transform.position.y / CHUNKSIZE));
            moveChunk.DrawChunk(mapData.data[moveChunkPos]);
        }

        beforeChunk = playerChunkPos;
    }
}
