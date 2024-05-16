using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Steak : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseHP(player);
    }
}
