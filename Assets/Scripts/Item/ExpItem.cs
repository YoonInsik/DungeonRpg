using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExpItem : BaseItem
{
    [SerializeField] int expAmount;

    protected override void Contact()
    {
        //Debug.Log("경험치 습득");
        GameManager.Instance.UpdateEXP(expAmount*UnitManager.Instance.player.WIsdomDelicacy());
        ReleaseObject();
    }
}
