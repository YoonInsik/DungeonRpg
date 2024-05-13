using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamburgerSteak : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseDEF(player);
        IncreaseATK(player);
    }

    public override void EndEffect(Player player)
    {
        IncreaseDEF(player, false);
        IncreaseATK(player, false);
    }
}
