using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaechuGuk : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseSpeed(player);
        IncreaseATKSpeed(player);
    }

    protected override void IncreaseATKSpeed(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["ATKSpeedLevel"] >= dict["StatMaxLevel"]) return;

        int Level = Random.Range(1, 3);

        ApplyOtherStat(dict, "ATKSpeedLevel", Level);
    }
}
