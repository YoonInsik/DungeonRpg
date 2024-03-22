using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Meat[] meats;
    //[SerializeField] private List<Item> items;

    public static Inventory instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
    }
    private void Start()
    {
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

    public Meat(MeatItem meatItem, int count)
    {
        this.meats = meatItem;
        this.count = count;
    }
}
