using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Udon : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseTemptation(player);
    }
}
