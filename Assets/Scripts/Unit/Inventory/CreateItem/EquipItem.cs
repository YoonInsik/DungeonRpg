using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Equip_", menuName = "item/equip")]
public class EquipItem : ScriptableObject
{
    public string ItemName;
    public Sprite icon = null;
}
