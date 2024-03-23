using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public Cooking[] cooking;
    [SerializeField] Meat[] meats;
    //[SerializeField] private List<Item> items;
    public List<CookingItem> cookingList;

    public static Inventory instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
    }
    private void Start()
    {
        cooking = new Cooking[5];
        //items = new List<Item>();
    }

    public void AddMeat(MeatItem meat)
    {
        if(meat.itemName == "beef")
        {
            Debug.Log("meet count");
            meats[0].count++;
        }
    }

    public void AddCookingItem(int number)
    {
        for (int i = 0; i < cooking.Length; i++)
        {
            if (cooking[i].cooking == null)
            {
                Debug.Log("비었다");
                cooking[i].cooking = cookingList[number];
                cooking[i].count++;
                return;
            }
            else if (cooking[i].cooking.itemName == cookingList[number].itemName)
            {
                Debug.Log("same name");
                cooking[i].count++;
                return;
            }
        }
        Debug.Log("더 이상 아이템을 추가할 수 없습니다.");
    }

    public Cooking[] getCookingItem()
    {
        return cooking;
    }

    public Meat[] GetMeats()
    {
        return meats;
    }
}

[Serializable]
public struct Meat
{
    public MeatItem meats;
    public int count;

    //public Meat(MeatItem meatItem, int count)
    //{
    //    this.meats = meatItem;
    //    this.count = count;
    //}
}

[Serializable]
public struct Cooking
{
    public CookingItem cooking;
    public int count;

    public Cooking(CookingItem cookingItem, int count)
    {
        this.cooking = cookingItem;
        this.count = count;
    }
}
