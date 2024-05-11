using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mille_FeuilleNabe : CookingItem
{
    public override void AddEffect(Player player)
    {
        HPRecovery(player);
        IncreaseGreed(player);
        IncreaseTemptation(player);
        IncreaseDelicacy(player);
    }
}
