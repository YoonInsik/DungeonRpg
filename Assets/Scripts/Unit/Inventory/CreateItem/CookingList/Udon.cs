using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Udon : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseTemptation(player);
        IncreaseCooldownReduction(player);
    }

    protected override void IncreaseCooldownReduction(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["CooldownReductionLevel"] >= dict["StatMaxLevel"]) return;

        int Level = Random.Range(1, 3);

        ApplyOtherStat(dict, "CooldownReductionLevel", Level);
    }
}
