using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baeksuk : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseATKSpeed(player);
        player.PlayerStatLevel.StatMaxLevel += 1;
    }
}
