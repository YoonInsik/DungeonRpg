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

    public void SelectLevelUpItems(List<ItemSelectPanel> options)
    {
        var needUpgradeItems = Inventory.Instance.itemDataList
            .Where(x => x.level < x.itemData.levelDamages.Count())
            .Select(x => x.itemData).ToList();

        var alreadyHaveItems = Inventory.Instance.itemDataList.Select(x => x.itemData).ToList();
        var newitems = itemDatas.Except(alreadyHaveItems).ToList();
        var randomItemPools = newitems.Union(needUpgradeItems).ToList();


        List<int> indexes = new List<int>();
        while (indexes.Count < 3)
        {
            if (indexes.Count >= randomItemPools.Count) break;

            var newRandNum = Random.Range(0, randomItemPools.Count);
            if (!indexes.Contains(newRandNum))
            {
                indexes.Add(newRandNum);
            }
        }

        for (int i = 0; i < indexes.Count; i++)
        {
            options[i].gameObject.SetActive(true);
            options[i].SetUI(randomItemPools[indexes[i]]);
        }
    }
}
