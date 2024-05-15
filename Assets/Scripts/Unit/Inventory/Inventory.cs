using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;

public class Inventory : Singleton<Inventory>
{
    //보유 아이템 리스트
    public List<PlayerItemData> itemDataList;

    //현재 갖고있는 요리아이템 저장
    [SerializeField] private Cooking[] cooking;
    [SerializeField] private Meat[] meats;
    //요리로 생성될 아이템 저장

    [SerializeField] private List<CookingItem> cookingList;

    public Player player;
    public CookingStatIncrease statUp;
    private void Start()
    {
        cooking = new Cooking[3];
        itemDataList = new List<PlayerItemData>();
        statUp = UnitManager.Instance.player.GetComponent<CookingStatIncrease>();
        player = UnitManager.Instance.player.GetComponent<Player>();
    }

    public void AddMeat(MeatItem meat)
    {
        switch (meat.itemName)
        {
            case "beef":
                meats[0].count++;
               break;
            case "pork":
                meats[1].count++;
                break;
            case "chicken":
                meats[2].count++;
                break;
            default:
                Debug.Log("can't add meat");
                break;
        }
    }

    public void AddCookingItem(CookingItem item)
    {
        int index = cookingList.IndexOf(item);
        Debug.Log(index);
    
        for (int i = 0; i < cooking.Length; i++)
        {
            if (cooking[i].cooking == null)
            {
                Debug.Log("비었다");
                cooking[i].cooking = cookingList[index];
                cooking[i].count++;
                return;
            }
            else if (cooking[i].cooking.name == cookingList[index].name)
            {
                Debug.Log("same name");
                cooking[i].count++;
                return;
            }
        }
        Debug.Log("더 이상 아이템을 추가할 수 없습니다.");
    }

    public void RemoveCookingItem(int index)
    {
        if (cooking[index].count == 1)
        {
            cooking[index] = new Cooking(null,0);
        }
        else
        {
            cooking[index] = new Cooking(cooking[index].cooking, cooking[index].count - 1);
        }
    }

    public Cooking[] GetCookingItem()
    {
        return cooking;
    }

    public Meat[] GetMeats()
    {
        return meats;
    }

    public List<CookingItem> GetCookingList()
    {
        return cookingList;
    }

    public void AddItemData(ItemData data)
    {
        if (itemDataList.Count > 6) return;

        var playerItem = itemDataList.Find(x => x.itemData.Equals(data));
        if (playerItem is null)
        {
            playerItem = new PlayerItemData();
            playerItem.itemData = data;
            itemDataList.Add(playerItem);

            foreach (Transform child in player.transform)
            {
                if (child.name.Equals(data.itemName))
                {
                    child.gameObject.SetActive(true);
                    print(child.name);
                }
            }
        }
         
        playerItem.level++;
        // item damage, stat level up
    }
}

[Serializable]
public struct Meat
{
    public MeatItem meats;
    public int count;
}

[Serializable]
public struct Cooking
{
    public CookingItem cooking;
    public int count;

    public Cooking(CookingItem cooking, int count)
    {
        this.cooking = cooking;
        this.count = count;
    }
}

public class PlayerItemData
{
    public int level;
    public ItemData itemData;
}
