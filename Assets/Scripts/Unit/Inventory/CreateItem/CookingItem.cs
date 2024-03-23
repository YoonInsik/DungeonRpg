using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cooking_", menuName = "item/cooking")]
public class CookingItem : ScriptableObject
{
    public string itemName;
    public Sprite icon = null;
}
