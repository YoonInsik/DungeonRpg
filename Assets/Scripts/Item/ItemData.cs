using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData_", menuName = "SO/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public ItemType itemTag;
    [TextArea]
    public string itemDesc;

    public float interval;
    public float speed;
    public float range;

    public float[] levelDamages;
}

[System.Flags]
public enum ItemType 
{ 
    무기,  
    재료,
    음식,
}
