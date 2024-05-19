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
        if (player.PlayerStatLevel.ATKSpeedLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = Random.Range(1, 3);

        if (player.PlayerStatLevel.ATKSpeedLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.ATKSpeedLevel = player.PlayerStatLevel.StatMaxLevel;
        }
        else player.PlayerStatLevel.ATKSpeedLevel += Level;
    }
}
