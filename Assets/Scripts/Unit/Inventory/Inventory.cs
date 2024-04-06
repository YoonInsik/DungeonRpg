using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : Singleton<Inventory>
{
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
                player.IncreaseFullness(cookingList[index].fullness);
                statUp.StatIncrease(cookingList[index]);
                return;
            }
            else if (cooking[i].cooking.itemName == cookingList[index].itemName)
            {
                Debug.Log("same name");
                cooking[i].count++;
                statUp.StatIncrease(cookingList[index]);
                player.IncreaseFullness(cookingList[index].fullness);
                return;
            }
        }
        Debug.Log("더 이상 아이템을 추가할 수 없습니다.");
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
}
