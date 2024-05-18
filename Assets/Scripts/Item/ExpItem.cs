using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExpItem : BaseItem
{
    [SerializeField] int expAmount;

    protected override void Contact()
    {
        //Debug.Log("����ġ ����");
        GameManager.Instance.UpdateEXP(expAmount*UnitManager.Instance.player.WIsdomDelicacy());
        ReleaseObject();
    }
}
