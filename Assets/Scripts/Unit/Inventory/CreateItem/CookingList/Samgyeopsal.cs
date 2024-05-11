using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samgyeopsal : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseATK(player);
    }

    public override void EndEffect(Player player)
    {
        IncreaseATK(player, false);
    }
}
