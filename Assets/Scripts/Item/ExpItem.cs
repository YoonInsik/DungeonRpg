using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExpItem : BaseItem
{
    [SerializeField] int expAmount;

    protected override void Contact()
    {
        Debug.Log("경험치 습득");
        UnitManager.Instance.player.IncreaseEXP(expAmount);
        ReleaseObject();
    }
}
