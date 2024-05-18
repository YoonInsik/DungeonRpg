using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Udon : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseTemptation(player);
    }
}
