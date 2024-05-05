using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public delegate IEnumerator CoAction();

    public const int MAXSTAGE = 5;
    
    public int Exp { get; private set; }
    public int MaxExp { get { return (Level + 3) * (Level + 3); } }
    public int Level { get; private set; }
    public float Timer { get; private set; }
    public int Stage { get; private set; }

    public CoAction levelUpAction;
    public List<ItemData> itemDatas;
    public List<ItemData> itemDataOptions;

    public LevelUpPanel levelUpPanel;
    public int levelUpAmount;

    void Start()
    {
        Init();

        UnitManager.Instance.SpawnPlayer(Vector2Int.zero);
        MapManager.Instance.InitMap();
    }

    private void Init() {
        Level = 1;
    }

    public IEnumerator StartTimer(int time)
    {
        Timer = time;

        while (Timer > 0)
        {
            Timer -= Time.deltaTime;
            yield return null;
        }
    }

    public void EnterChunk()
    {
        Stage++;
        levelUpAmount = 0;
        MapManager.Instance.ReloadChunks();
        MapManager.Instance.CurChunk.InvokeEvent();
    }

    public void UpdateEXP(int amount)
    {
        Exp += amount;

        if (Exp >= MaxExp) {
            Exp = Exp - MaxExp;

            Level++;
            levelUpAmount++;
        }
    }

    public void SelectLevelUpItems(int optionNum)
    {
        if (itemDatas.Count < optionNum) return;

        List<int> indexes = new List<int>();
        while (indexes.Count < optionNum)
        {
            var newRandNum = Random.Range(0, itemDatas.Count);
            if (!indexes.Contains(newRandNum))
            {
                indexes.Add(newRandNum);
            }
        }

        itemDataOptions.Clear();
        foreach (var index in indexes)
        {
            itemDataOptions.Add(itemDatas[index]);
        }
    }
}
