using SHS;
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
        // �� ����
        MapManager.Instance.CurChunk.SetActiveDoorTilemap(false);
        MapManager.Instance.SetActiveNextChunkUI(true);

        yield break;
    }

    public IEnumerator NormalChunkEvent()
    {
        // �� �ݱ�
        MapManager.Instance.CurChunk.SetActiveDoorTilemap(true);
        MapManager.Instance.SetActiveNextChunkUI(false);

        // �����
        var decreaseFullnessCo = StartCoroutine(UnitManager.Instance.player.DecreaseFullness());

        // 요리 버프 지속시간 시작
        UnitManager.Instance.player.pause = false;
        // ���� �ݺ� ��ȯ
        //var enemySpawnCo = StartCoroutine(SHS.EnemySpawner.Instance.EnemySpawn_Coroutine(1f));
        //  ----> EnemySpawner_v3�� ���̺� ����� ���� �Է�
        EnemySpawner_v3.Instance.MakeWave();

        yield return StartCoroutine(GameManager.Instance.StartTimer(30));

        // �ð� ����
        StopCoroutine(decreaseFullnessCo);

        // 요리 버프 지속 일시 정지
        UnitManager.Instance.player.pause = true;
        //StopCoroutine(enemySpawnCo);
        //  ----> ���̺갡 ������� �� ���̺� ũ��(�ð��� �ֱ⿡ ���� ����) ������ �־� ���� ������ ��� ��. ���� ���Ͱ� ���� �ÿ��� ���� ������ ��������.

        // �� ���� �Լ�
        /*
        foreach (var enemy in EnemyQueueManager.instance.EnemyQueue)
        {
            EnemyQueueManager.ReturnObject(enemy);
        }
        EnemyQueueManager에서 정리하게 명령
        */
        EnemyQueueManager.instance.ClearMonster();
        UnitManager.Instance.enemies.Clear();

        // �� Ŭ���� ����
        GameManager.Instance.levelUpPanel.PopUpLevelUpPanel();
        yield return new WaitUntil(() => GameManager.Instance.levelUpAmount <= 0);

        // 요리를 할 화로 생성 및 UI 활성화
        UnitManager.Instance.player.InstantiateFurnace();

        // �� ����
        MapManager.Instance.CurChunk.SetActiveDoorTilemap(false);
        MapManager.Instance.SetActiveNextChunkUI(true);
    }
}