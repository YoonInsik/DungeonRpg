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

    public List<GameObject> droppedObjList;

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
                chunkEvent = BossChunkEvent;
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
        droppedObjList = new List<GameObject>();
        MapManager.Instance.CurChunk.SetActiveDoorTilemap(false);
        MapManager.Instance.SetActiveNextChunkUI(true);

        yield break;
    }

    public IEnumerator NormalChunkEvent()
    {
        // �� �ݱ�
        droppedObjList = new List<GameObject>();
        MapManager.Instance.CurChunk.SetActiveDoorTilemap(true);
        MapManager.Instance.SetActiveNextChunkUI(false);

        // �����
        var decreaseFullnessCo = StartCoroutine(UnitManager.Instance.player.DecreaseFullness());

        // 체력 재생
        var recoveryCo = StartCoroutine(Player_HpManager.instance.RecoveryPerSec(1));

        // 요리 버프 지속시간 시작
        UnitManager.Instance.player.pause = false;

        EnemySpawner_v3.Instance.MakeWave();

        yield return StartCoroutine(GameManager.Instance.StartTimer(25 + 5 * GameManager.Instance.Stage));
        if (UnitManager.Instance.player.isDead)
            yield break;

        // �ð� ����
        if (decreaseFullnessCo != null)
            StopCoroutine(decreaseFullnessCo);
        

        // 요리 버프 지속 일시 정지
        UnitManager.Instance.player.pause = true;

        StopCoroutine(recoveryCo);

        EnemyQueueManager.instance.ClearMonster();
        UnitManager.Instance.enemies.Clear();

        foreach (var obj in droppedObjList)
        {
            obj.GetComponent<Poolable>().ReleaseObject();
        }

        // �� Ŭ���� ����
        GameManager.Instance.levelUpPanel.PopUpLevelUpPanel();
        yield return new WaitUntil(() => GameManager.Instance.levelUpAmount <= 0);

        // 요리를 할 화로 생성 및 UI 활성화
        UnitManager.Instance.player.InstantiateFurnace();

        // �� ����
        MapManager.Instance.CurChunk.SetActiveDoorTilemap(false);
        MapManager.Instance.SetActiveNextChunkUI(true);
    }

    public IEnumerator BossChunkEvent()
    {
        // �� �ݱ�
        droppedObjList = new List<GameObject>();
        MapManager.Instance.CurChunk.SetActiveDoorTilemap(true);
        MapManager.Instance.SetActiveNextChunkUI(false);

        // �����
        var decreaseFullnessCo = StartCoroutine(UnitManager.Instance.player.DecreaseFullness());

        // 체력 재생
        var recoveryCo = StartCoroutine(Player_HpManager.instance.RecoveryPerSec(1));

        // 요리 버프 지속시간 시작
        UnitManager.Instance.player.pause = false;

        EnemySpawner_v3.Instance.MakeWave();

        yield return StartCoroutine(GameManager.Instance.StartTimer(25 + 5 * GameManager.Instance.Stage));
        if (UnitManager.Instance.player.isDead)
            yield break;

        // �ð� ����
        if (decreaseFullnessCo != null)
            StopCoroutine(decreaseFullnessCo);


        // 요리 버프 지속 일시 정지
        UnitManager.Instance.player.pause = true;

        StopCoroutine(recoveryCo);

        EnemyQueueManager.instance.ClearMonster();
        UnitManager.Instance.enemies.Clear();

        foreach (var obj in droppedObjList)
        {
            obj.GetComponent<Poolable>().ReleaseObject();
        }

        //보스룸 끝
        GameOverPanel.instance.Object_On(true);
    }
}