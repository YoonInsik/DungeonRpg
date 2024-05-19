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
        if (player.PlayerStatLevel.CooldownReductionLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = Random.Range(1, 3);

        if (player.PlayerStatLevel.CooldownReductionLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.CooldownReductionLevel = player.PlayerStatLevel.StatMaxLevel;
        }
        else player.PlayerStatLevel.CooldownReductionLevel += Level;
    }
}
