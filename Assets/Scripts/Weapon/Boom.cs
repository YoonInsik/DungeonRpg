using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : WeaponBase
{
    private void Awake()
    {
        baseDamage = 10.0f;
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float damage = CalculateDamage();
            collision.GetComponent<Enemy>().Damaged(damage);
        }
    }
}
