using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSkewers : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseCooldownReduction(player);
        IncreaseWisdom(player);
    }

    public override void EndEffect(Player player)
    {
        IncreaseWisdom(player, false);
    }
}
