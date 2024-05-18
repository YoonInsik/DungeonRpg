using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jeyuk_bokkeum : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseATK(player);
    }
}
