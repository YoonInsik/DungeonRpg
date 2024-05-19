using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricField : WeaponBase
{
    private List<Collider2D> enemys = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!enemys.Contains(collision))
            {
                enemys.Add(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (enemys.Contains(collision))
            {
                enemys.Remove(collision);
            }
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > data.interval)
        {
            if(enemys != null)
            {
                foreach (Collider2D collision in enemys)
                {
                    float damage = CalculateDamage();
                    collision.GetComponent<Enemy>().Damaged(damage);
                }
            }
            elapsedTime = 0.0f;
        }
    }
}
