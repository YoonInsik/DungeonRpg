using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModumSpecialGui : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseAll(player);
        IncreaseAll(player);
    }

    public override void EndEffect(Player player)
    {
        IncreaseAll(player, false);
    }
}
