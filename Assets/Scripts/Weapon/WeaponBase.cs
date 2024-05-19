using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public int weaponLevel = 0;
    public float baseDamage;
    public Vector2 attackScale;

    public float CalculateDamage()
    {
        switch (weaponLevel)
        {
            case 1:
                return baseDamage;
            case 2:
                return baseDamage * 1.5f;
            case 3:
                return baseDamage * 2.0f;
            default:
                return baseDamage;
        }
    }

    public void LevelUp()
    {
        weaponLevel++;
    }
}