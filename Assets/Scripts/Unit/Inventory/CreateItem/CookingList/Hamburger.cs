using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamburger : CookingItem
{
    public override IEnumerator CookingEffect(Player player)
    {
        AddEffect(player);
        float elapsedtime = 0f;

        while (elapsedtime < buffDuration)
        {
            if (!player.pause)
            {
                HPRecovery(player);
                elapsedtime++;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    protected override void AddEffect(Player player)
    {
        IncreaseSpeed(player);
        Debug.Log("ÇÜ¹ö°Å Àû¿ë");
    }

}

