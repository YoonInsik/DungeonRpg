using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Malatang : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseDelicacy(player);
        IncreaseFullnessDecrease(player);
    }
}
