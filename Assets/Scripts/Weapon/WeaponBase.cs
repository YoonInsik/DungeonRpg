using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public int level;
    public int weaponCount;
    public float damage;

    public void LevelUp(float damage, int weaponCount)
    {
        this.damage = damage;
        this.weaponCount = weaponCount;

        if(weaponCount == 1)
        {
            damage = damage * 1.2f;
        }
        else if(weaponCount == 2)
        {
            damage = damage * 1.4f;
        }
        else if (weaponCount == 3)
        {
            damage = damage * 1.6f;
        }
        else if (weaponCount == 4)
        {
            damage = damage * 1.8f;
        }
        else if (weaponCount == 5)
        {
            damage = damage * 2.0f;
        }
    }
}
