using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSalad : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseCooldownReduction(player);
        IncreaseSpeed(player);
    }

    public override void EndEffect(Player player)
    {
        IncreaseSpeed(player, false);
    }
}
