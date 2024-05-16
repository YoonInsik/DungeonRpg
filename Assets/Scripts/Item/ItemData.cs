using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData_", menuName = "SO/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public ItemType itemTag;
    public string itemDesc;

    public float baseDamage;
    public int baseCount;
    public float[] levelDamages;
    public int[] levelCounts;
}

[System.Flags]
public enum ItemType 
{ 
    Weapon,  
    Ingrediant,
    Dish,
}
