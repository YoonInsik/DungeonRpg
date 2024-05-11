using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jokbal : CookingItem
{
    public override void AddEffect(Player player)
    {
        IncreaseWisdom(player);
        IncreaseATKRange(player);
    }
}
