using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSalad : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseCooldownReduction(player);
        IncreaseSpeed(player);
    }

    protected override void EndEffect(Player player)
    {
        IncreaseSpeed(player, false);
    }
}
