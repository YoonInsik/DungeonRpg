using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Boss_A : MonoBehaviour
    {
        //변수
        EnemyStat m_stat;
        Transform player_trns;
        Rigidbody2D m_rigid;
        Enemy m_enemy;
        Animator m_ani;

        // Start is called before the first frame update
        void Start()
        {
            m_enemy = GetComponent<Enemy>();
            m_stat = GetComponent<Enemy>().Get_MyStat();
            m_rigid = GetComponent<Rigidbody2D>();
            player_trns = GameObject.FindGameObjectWithTag("Player").transform;
            m_ani = GetComponent<Animator>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (m_enemy.now_burrow)
                return;

            if (now_attack)
                return;

            Move();
        }


        //플레이어로 향하는 방향
        Vector2 Direction()
        {
            Vector2 dir = player_trns.position - transform.position ;

            return dir.normalized;
        }

        // 움직임
        void Move()
        {
            m_rigid.velocity = m_enemy.now_speed * Direction();

            remain = Vector2.Distance(player_trns.position, transform.position);

            if (remain < Attack1_length)
            {
                now_attack = true;
                m_rigid.velocity = Vector2.zero;
                GroundSmash();
            }
        }

        //플레이어와 몬스터의 거리
        [SerializeField] float remain;
        [SerializeField] bool now_attack;

        [Header("땅찍기")]
        [SerializeField] float Attack1_length;
        [SerializeField] GameObject smash_effect;

        //그라운드 스매쉬
        void GroundSmash()
        {
            m_ani.SetTrigger("groundsmash");
        }

        void Ani_GroundSmash_Effect()
        {
            Instantiate(smash_effect, transform.position, Quaternion.identity);
        }

        void Ani_GroundSmash_End()
        {
            now_attack = false;
        }
    }

}