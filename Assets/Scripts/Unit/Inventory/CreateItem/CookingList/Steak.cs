using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steak : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseHP(player);
    }
}
