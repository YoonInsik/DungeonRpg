using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS 
{
    public class EnemyAttackField : MonoBehaviour
    {
        //�ν��Ͻ�
        Player_HpManager pd;
        [SerializeField] Enemy m_enemy;


        private void Start()
        {
            pd = Player_HpManager.instance;
        }

        [Header("���� ����")]
        [SerializeField] bool can_attack;
        [SerializeField] LayerMask player_layer;
        [SerializeField] float attack_radius = 1f;
        [SerializeField] float attack_cooltime_set;

        [Header("���� ���� - Ȯ�ο�")]
        [SerializeField] float attack_cooltime;

        private void Update()
        {
            if (attack_cooltime > 0)
            {
                attack_cooltime -= Time.deltaTime;
                return;
            }

            if (can_attack && !m_enemy.now_burrow)
            {
                if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), attack_radius, player_layer))
                {
                    attack_cooltime = attack_cooltime_set;
                    pd.Damaged(m_enemy.m_stat.damage + m_enemy.m_stat.damage_scaling * (EnemySpawner_v3.Instance.Get_WaveLevel() - 1));
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attack_radius);
        }

    }
}