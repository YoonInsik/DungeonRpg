using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS 
{
    public class EnemyAttackField : MonoBehaviour
    {
        //인스턴스
        Player_HpManager pd;
        public Enemy m_enemy;


        private void Start()
        {
            pd = Player_HpManager.instance;
        }

        [Header("공격 세팅")]
        [SerializeField] bool can_attack;
        [SerializeField] bool will_destroy;
        [SerializeField] LayerMask player_layer;
        [SerializeField] float attack_radius = 1f;
        [SerializeField] float attack_cooltime_set;

        [Header("공격 세팅 - 확인용")]
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

                    if (will_destroy)
                    {
                        Destroy(gameObject);
                    }
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