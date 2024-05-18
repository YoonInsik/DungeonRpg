using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class High_class_steak : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseHP(player);
        HPRecovery(player, true);
    }
}
