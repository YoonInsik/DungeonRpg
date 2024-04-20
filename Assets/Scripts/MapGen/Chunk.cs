using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap propTilemap;
    [SerializeField] private Tilemap wallTilemap;

    public ChunkData data;

    private delegate IEnumerator ChunkDelegate();
    private ChunkDelegate chunkEvent;

    public void DrawChunk()
    {
        MapManager.Instance.tilemapVisualizer.PaintTiles(floorTilemap);
    }

    public void SetActiveDoorTilemap(bool value)
    {
        propTilemap.gameObject.SetActive(value);
    }

    public void InvokeEvent()
    {
        if (chunkEvent is null) return;
        StartCoroutine(chunkEvent.Invoke());
    }

    public void SetChunkDelegate()
    {
        switch (data.chunkType)
        {
            case ChunkType.Start:
                chunkEvent = StartChunkEvent;
                break;
            case ChunkType.Boss:
                chunkEvent = NormalChunkEvent;
                break;
            case ChunkType.Normal:
                chunkEvent = NormalChunkEvent;
                break;
            case ChunkType.Many:
                chunkEvent = NormalChunkEvent;
                break;
            default:
                break;
        }
    }

    public IEnumerator StartChunkEvent()
    {
        // 방 열기
        MapManager.Instance.CurChunk.SetActiveDoorTilemap(false);
        MapManager.Instance.SetActiveNextChunkUI(true);

        yield break;
    }

    public IEnumerator NormalChunkEvent()
    {
        // 방 닫기
        MapManager.Instance.CurChunk.SetActiveDoorTilemap(true);
        MapManager.Instance.SetActiveNextChunkUI(false);

        // 배고픔
        var decreaseFullnessCo = StartCoroutine(UnitManager.Instance.player.DecreaseFullness());

        // 몬스터 반복 소환
        var enemySpawnCo = StartCoroutine(SHS.EnemySpawner.Instance.EnemySpawn_Coroutine(1f));
        yield return StartCoroutine(GameManager.Instance.StartTimer(60));

        // 시간 종료
        StopCoroutine(decreaseFullnessCo);
        StopCoroutine(enemySpawnCo);

        // 적 제거 함수
        foreach (var enemy in UnitManager.Instance.enemies)
        {
            SHS.EnemySpawner.ReturnObject(enemy);
        }
        UnitManager.Instance.enemies.Clear();

        // 방 클리어 보상


        // 방 열기
        MapManager.Instance.CurChunk.SetActiveDoorTilemap(false);
        MapManager.Instance.SetActiveNextChunkUI(true);
    }
}