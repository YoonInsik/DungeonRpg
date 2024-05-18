using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samgyeopsal : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseATK(player);
    }

    protected override void EndEffect(Player player)
    {
        IncreaseATK(player, false);
    }
}
