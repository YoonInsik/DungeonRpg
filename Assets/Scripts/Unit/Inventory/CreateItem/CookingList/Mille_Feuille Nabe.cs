using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mille_FeuilleNabe : CookingItem
{
    protected override void AddEffect(Player player)
    {
        IncreaseHP(player);
        IncreaseGreed(player);
        IncreaseTemptation(player);
        IncreaseDelicacy(player);
    }
}
