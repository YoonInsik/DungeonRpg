using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jokbal : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseWisdom(player);
        IncreaseATKRange(player);
    }
}
