using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModumSpecialGui : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseAll(player);
        IncreaseAll(player);
    }

    protected override void EndEffect(Player player)
    {
        IncreaseAll(player, false);
    }
}
