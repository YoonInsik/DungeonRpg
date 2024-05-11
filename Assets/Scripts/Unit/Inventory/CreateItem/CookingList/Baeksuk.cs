using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baeksuk : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseATKSpeed(player);
    }

    public override void EndEffect(Player player)
    {
        IncreaseATKSpeed(player, false);
    }
}
