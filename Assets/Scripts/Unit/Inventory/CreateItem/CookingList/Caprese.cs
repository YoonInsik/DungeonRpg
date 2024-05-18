using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caprese : CookingItem
{
    public override void IncreaseATK(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int DelicacyRate = player.PlayerStatLevel.DelicacyLevel * 10;

        int change = isPositive ? player.PlayerStatLevel.SpeedLevel / 3 : -player.PlayerStatLevel.SpeedLevel / 3;
        player.ATK += change + DelicacyRate;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.ATKLevel += Level;
    }

    public override void AddEffect(Player player)
    {
        IncreaseSpeed(player);
        IncreaseATK(player);
    }
}
