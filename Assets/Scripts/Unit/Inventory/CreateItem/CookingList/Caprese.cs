using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caprese : CookingItem
{
    protected override void IncreaseATK(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["ATKLevel"] >= dict["StatMaxLevel"]) return;

        int DelicacyRate = dict["DelicacyLevel"] * 10;

        int change = isPositive ? dict["SpeedLevel"] / 3 : -dict["SpeedLevel"] / 3;
        player.ATK += change + DelicacyRate;

        int Level = isPositive ? 1 : -1;
        dict["ATKLevel"] += Level;
    }

    protected override void AddEffect(Player player)
    {
        IncreaseSpeed(player);
        IncreaseATK(player);
    }
}
