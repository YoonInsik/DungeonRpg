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

    public void SetPropTilemap(bool value)
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
                chunkEvent = EventStart;
                break;
            case ChunkType.Normal:
                chunkEvent = EventStart;
                break;
            case ChunkType.Many:
                chunkEvent = EventStart;
                break;
            default:
                break;
        }
    }

    public IEnumerator StartChunkEvent()
    {
        // 방 열기
        MapManager.Instance.CurChunk.SetPropTilemap(false);
        MapManager.Instance.ActiveChunkUI(true);

        yield break;
    }

    public IEnumerator EventStart()
    {
        // 방 닫기
        MapManager.Instance.CurChunk.SetPropTilemap(true);
        MapManager.Instance.ActiveChunkUI(false);

        // 몬스터 반복 소환
        var co = StartCoroutine(SHS.EnemySpawner.Instance.EnemySpawn_Coroutine(1f));

        yield return new WaitForSeconds(3f);
        StopCoroutine(co);

        // 적 제거 함수
        foreach (var enemy in UnitManager.Instance.enemies)
        {
            SHS.EnemySpawner.ReturnObject(enemy);
        }
        UnitManager.Instance.enemies.Clear();

        // 방 클리어 보상


        // 방 열기
        MapManager.Instance.CurChunk.SetPropTilemap(false);
        MapManager.Instance.ActiveChunkUI(true);
    }
}