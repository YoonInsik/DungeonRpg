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

        Debug.Log(buffDuration);
        Debug.Log(HP);
        while (elapsedtime < buffDuration)
        {
            if (!player.pause)
            {
                Debug.Log(elapsedtime);
                HPRecovery(player);
                elapsedtime++;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public override void AddEffect(Player player)
    {
        IncreaseSpeed(player);
        Debug.Log("ÇÜ¹ö°Å Àû¿ë");
    }

}

