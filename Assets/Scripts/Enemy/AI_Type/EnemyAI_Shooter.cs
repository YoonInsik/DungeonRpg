using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Shooter : MonoBehaviour
    {
        //변수
        Transform player_trns;
        Rigidbody2D m_rigid;
        Enemy m_enemy;
        

        // Start is called before the first frame update
        void Start()
        {
            m_enemy = GetComponent<Enemy>();
            m_rigid = GetComponent<Rigidbody2D>();
            player_trns = GameObject.FindGameObjectWithTag("Player").transform;

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (m_enemy.now_burrow)
                return;

            if(m_enemy.Get_NowHP() <= 0 && save_attack != null)
            {
                Destroy(save_attack);
                save_attack = null;
            }

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
                m_rigid.velocity = m_enemy.now_speed * Direction();

        }

        [Header("공격")]
        [SerializeField] Transform attack_center;
        [SerializeField] bool now_attack;
        [SerializeField] float snipe_time;
        [SerializeField] GameObject snipe_attackgroup;

        GameObject save_attack;

        IEnumerator Sniping()
        {
            now_attack = true;

            save_attack = Instantiate(snipe_attackgroup, attack_center);
            save_attack.transform.parent = null;
            save_attack.GetComponentInChildren<EnemyAttackBullet>().damamge = m_enemy.m_stat.damage + m_enemy.m_stat.damage_scaling * (EnemySpawner_v3.Instance.Get_WaveLevel() - 1);

            yield return new WaitForSeconds(snipe_time);

            if(save_attack != null)
                save_attack = null;

            now_attack = false;
        }
    }

}