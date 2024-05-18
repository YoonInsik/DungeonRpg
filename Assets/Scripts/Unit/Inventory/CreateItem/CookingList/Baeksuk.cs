using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baeksuk : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseATKSpeed(player);
        player.PlayerStatLevel.StatMaxLevel += 1;
    }
}
