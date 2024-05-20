using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baeksuk : CookingItem
{
    protected override void AddEffect(Player player)
    {
        Dictionary<string, int> dict = player.StatLevels;

        IncreaseATKSpeed(player);
        dict["StatMaxLevel"] += 1;
    }
}
