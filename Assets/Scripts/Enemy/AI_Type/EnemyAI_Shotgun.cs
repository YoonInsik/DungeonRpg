using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Shotgun : MonoBehaviour
    {
        //변수
        EnemyStat m_stat;
        Transform player_trns;
        Rigidbody2D m_rigid;
        Enemy m_enemy;
        

        // Start is called before the first frame update
        void Start()
        {
            m_enemy = GetComponent<Enemy>();
            m_stat = GetComponent<Enemy>().Get_MyStat();
            m_rigid = GetComponent<Rigidbody2D>();
            player_trns = GameObject.FindGameObjectWithTag("Player").transform;

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (m_enemy.now_burrow)
                return;

            Move();
        }


        //플레이어로 향하는 방향
        Vector2 Direction()
        {
            Vector2 dir = player_trns.position - transform.position ;

            return dir.normalized;
        }

        [Header("움직임")]
        [SerializeField] Animator m_model_ani;
        [SerializeField] float snipe_length;

        [Header("움직임_확인")]
        [SerializeField] float remain; 

        // 움직임
        void Move()
        {
            attack_center.LookAt(player_trns);

            m_model_ani.SetBool("snipe", now_attack);

            if (now_attack)
                return;

            remain = Vector2.Distance(player_trns.position, transform.position);

            if (snipe_length * 0.85f > remain)
            {
                m_rigid.velocity = Vector2.zero;
                StartCoroutine(Sniping());
            }
            else
                m_rigid.velocity = m_stat.speed * Direction();

        }

        [Header("공격")]
        [SerializeField] Transform attack_center;
        [SerializeField] bool now_attack;
        [SerializeField] float snipe_time;
        [SerializeField] GameObject snipe_attackgroup;

        IEnumerator Sniping()
        {
            now_attack = true;

            GameObject C = Instantiate(snipe_attackgroup, attack_center);
            C.transform.parent = null;
            C.GetComponentInChildren<EnemyAttackBullet>().damamge = m_enemy.m_stat.damage + m_enemy.m_stat.damage_scaling * (EnemySpawner_v3.Instance.Get_WaveLevel() - 1);

            yield return new WaitForSeconds(snipe_time);

            now_attack = false;
        }
    }

}