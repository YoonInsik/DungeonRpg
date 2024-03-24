using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    [Header("적들")]
    [SerializeField] LayerMask ENEMY_LAYER;

    // Update is called once per frame
    void Update()
    {
        Damaging();
    }

    [Header("피해")]
    [SerializeField] float damage;
    [SerializeField] float damage_time_set;

    [Header("피해 - 수치확인")]
    [SerializeField] float damage_time;
    [SerializeField] Collider2D[] enemys;
    void Damaging()
    {

        enemys = Physics2D.OverlapCircleAll(new Vector2( transform.position.x, transform.position.y), 3f, ENEMY_LAYER);

        if (damage_time > 0)
        {
            damage_time -= Time.deltaTime;
        }
        else
        {
            damage_time = damage_time_set;

            if (enemys.Length > 0)
            {
                for(int i = 0; i< enemys.Length; i++)
                {
                    enemys[i].GetComponent<Enemy>().Damaged(damage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}
