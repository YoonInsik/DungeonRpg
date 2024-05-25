using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Skater : MonoBehaviour
    {
        //����
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

            now_dir = (player_trns.position - transform.position).normalized;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (m_enemy.now_burrow)
                return;

            Move();
        }


        //�÷��̾�� ���ϴ� ����
        Vector2 Direction()
        {
            now_dir = Vector2.Lerp(now_dir, (player_trns.position - transform.position).normalized, skating_rate);

            return now_dir.normalized;
        }

        // ������
        [Header("�̲�������")]
        [SerializeField] float skating_rate = 0.1f;
        Vector2 now_dir;

        void Move()
        {
            m_rigid.velocity = m_stat.speed * Direction();

        }

    }

}