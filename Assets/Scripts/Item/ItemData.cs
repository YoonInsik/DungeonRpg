using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData_", menuName = "SO/ItemData")]
public class ItemData : ScriptableObject
{
    public int itemCode;
    public GameObject itemPrefab;
    public Sprite itemImage;
    public string itemName;
    public ItemType itemTag;
    public string itemDesc;
}

[System.Flags]
public enum ItemType 
{ 
    Weapon,  
    Ingrediant,
    Dish,
}
