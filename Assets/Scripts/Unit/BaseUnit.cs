using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    [SerializeField] protected BaseStat baseStat;

    public int HP { get; set; }
    public int ATK { get; set; }
    public int DEF { get; set; }

    protected void Start()
    {
        InitStat();
    }

    private void InitStat()
    {
        HP = baseStat.baseHP;
        ATK = baseStat.baseATK;
        DEF = baseStat.baseDEF;
    }
}
