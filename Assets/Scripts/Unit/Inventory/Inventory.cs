using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : Singleton<Inventory>
{
    //���� �����ִ� �丮������ ����
    [SerializeField] private Cooking[] cooking;
    [SerializeField] private Meat[] meats;
    //�丮�� ������ ������ ����
    [SerializeField] private List<CookingItem> cookingList;
    public CookingStatIncrease statUp;
    private void Start()
    {
        cooking = new Cooking[5];
        statUp = GameObject.FindWithTag("Player").GetComponent<CookingStatIncrease>();
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
                Debug.Log("�����");
                cooking[i].cooking = cookingList[number];
                cooking[i].count++;
                statUp.StatIncrease(cookingList[number]);
                return;
            }
            else if (cooking[i].cooking.itemName == cookingList[number].itemName)
            {
                Debug.Log("same name");
                cooking[i].count++;
                statUp.StatIncrease(cookingList[number]);
                return;
            }
        }
        Debug.Log("�� �̻� �������� �߰��� �� �����ϴ�.");
    }

    public Cooking[] GetCookingItem()
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
}

[Serializable]
public struct Cooking
{
    public CookingItem cooking;
    public int count;
}
