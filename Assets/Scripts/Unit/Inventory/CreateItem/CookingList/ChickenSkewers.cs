using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSkewers : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseCooldownReduction(player);
        IncreaseWisdom(player);
    }

    protected override void EndEffect(Player player)
    {
        IncreaseWisdom(player, false);
    }
}
