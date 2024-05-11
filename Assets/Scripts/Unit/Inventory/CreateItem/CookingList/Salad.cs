using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salad : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseSpeed(player);
    }

}
