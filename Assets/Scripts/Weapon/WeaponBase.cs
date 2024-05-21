using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public ItemData data;

    protected Player player;
    protected int weaponLevel = 0;
    protected float elapsedTime = 0.0f;

    public Vector2 attackScale;

    public float CalculateDamage()
    {
        return data.levelDamages[weaponLevel - 1];
    }

    public void LevelUp()
    {
        weaponLevel++;
    }
}

public enum SwordState
{
    Scan,
    Attack,
    Return
}