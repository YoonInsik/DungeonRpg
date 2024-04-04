using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cooking_", menuName = "item/cooking")]
public class CookingItem : ScriptableObject
{
    public string itemName;
    public Sprite icon = null;
    public float fullness;

    public int ATK;
    public int SPEED;
    public int HP;
    public int DEF;
}
