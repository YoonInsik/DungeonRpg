using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private MapData mapData; // for save

    [SerializeField] private Transform chunkContainer;
    [SerializeField] private Chunk chunkPrefab;

    [SerializeField] private TextMeshProUGUI rightChunkText;
    [SerializeField] private TextMeshProUGUI downChunkText;
    [SerializeField] private TextMeshProUGUI leftChunkText;
    [SerializeField] private TextMeshProUGUI upChunkText;

    public TilemapVisualizer tilemapVisualizer;
    public Vector2Int beforeChunkPos;

    public const int CHUNKSIZE = 20;
    public const int HALFCHUNKSIZE = 10;

    [SerializeField] private List<Chunk> chunkList;
    private Chunk curChunk;

    public Chunk CurChunk { get => curChunk; }

    public void InitMap()
    {
        SpawnChunks();
        ReloadChunks();
        CurChunk.InvokeEvent();
    }

    private void SetChunkData(Chunk chunk)
    {
        chunk.data = new ChunkData();
        chunk.data.dropTable.Add(DropManager.Instance.GetRandomDropData());

        switch (GameManager.Instance.Stage)
        {
            case 0:
                if (chunk.transform.position == Vector3.zero)
                    chunk.data.chunkType = ChunkType.Start;
                else
                    chunk.data.chunkType = (ChunkType)Random.Range((int)ChunkType.Normal, System.Enum.GetValues(typeof(ChunkType)).Length);
                break;
            case GameManager.MAXSTAGE - 1:
                chunk.data.chunkType = ChunkType.Boss;
                break;
            default:
                chunk.data.chunkType = (ChunkType)Random.Range((int)ChunkType.Normal, System.Enum.GetValues(typeof(ChunkType)).Length);
                break;
        }
    }

    private void SpawnChunks()
    {
        for (int y = 1; y >= -1; y--)
        {
            for (int x = -1; x <= 1; x++)
            {
                var newChunk = Instantiate(chunkPrefab, chunkContainer);
                newChunk.name = $"Chunk {x} {y}";
                newChunk.transform.position = new Vector3(CHUNKSIZE * x, CHUNKSIZE * y);
                chunkList.Add(newChunk);
            }
        }

        beforeChunkPos = Vector2Int.zero;
        curChunk = chunkList[4];
    }

    public void ReloadChunks()
    {
        Vector2 playerPos = UnitManager.Instance.player.transform.position;

        Vector2Int playerChunkPos = new Vector2Int(
            Mathf.RoundToInt(playerPos.x / CHUNKSIZE),
            Mathf.RoundToInt(playerPos.y / CHUNKSIZE));

        Vector2Int dir = playerChunkPos - beforeChunkPos;
        foreach (var chunkItem in chunkList)
        {
            Vector2Int moveVec = Vector2Int.zero;
            if (Mathf.Abs(dir.x) > 0)
            {
                var moveChunkVec = chunkItem.transform.position.x - beforeChunkPos.x * CHUNKSIZE;
                if (moveChunkVec != 0 && Mathf.Sign(moveChunkVec) != Mathf.Sign(dir.x))
                {
                    moveVec += dir;
                }
            }
            if (Mathf.Abs(dir.y) > 0)
            {
                var moveChunkVec = chunkItem.transform.position.y - beforeChunkPos.y * CHUNKSIZE;
                if (moveChunkVec != 0 && Mathf.Sign(moveChunkVec) != Mathf.Sign(dir.y))
                {
                    moveVec += dir;
                }
            }

            if (moveVec != Vector2Int.zero)
            {
                chunkItem.transform.Translate(new Vector2(moveVec.x, moveVec.y) * CHUNKSIZE * 3);
            }
            SetChunkData(chunkItem);

            Vector2Int moveChunkPos = new Vector2Int(
                Mathf.RoundToInt(chunkItem.transform.position.x / CHUNKSIZE),
                Mathf.RoundToInt(chunkItem.transform.position.y / CHUNKSIZE));

            chunkItem.DrawChunk();
            chunkItem.SetChunkDelegate();
            if (moveChunkPos == playerChunkPos) curChunk = chunkItem;

            SetChunkUI(chunkItem, moveChunkPos, playerChunkPos);
        }

        beforeChunkPos = playerChunkPos;
    }

    private void SetChunkUI(Chunk chunkItem, Vector2Int moveChunkPos, Vector2Int playerChunkPos)
    {
        if ((moveChunkPos - playerChunkPos) == Vector2Int.right)
        {
            rightChunkText.text = chunkItem.data.chunkType.ToString();
        }
        else if ((moveChunkPos - playerChunkPos) == Vector2Int.down)
        {
            downChunkText.text = chunkItem.data.chunkType.ToString();
        }
        else if ((moveChunkPos - playerChunkPos) == Vector2Int.left)
        {
            leftChunkText.text = chunkItem.data.chunkType.ToString();
        }
        else
        {
            upChunkText.text = chunkItem.data.chunkType.ToString();
        }
    }

    public void ActiveChunkUI(bool value)
    {
        rightChunkText.gameObject.SetActive(value);
        downChunkText.gameObject.SetActive(value);
        leftChunkText.gameObject.SetActive(value);
        upChunkText.gameObject.SetActive(value);
    }
}