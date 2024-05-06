using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Seperater : MonoBehaviour
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
            Vector2 dir = player_trns.position - transform.position ;

            return dir.normalized;
        }

        // ������
        void Move()
        {
            m_rigid.velocity = m_stat.speed * Direction();

        }

        [Header("�п�")]
        [SerializeField] GameObject seperate_obj;

        public void Seperate()
        {
            Instantiate(seperate_obj, transform.position + Vector3.right * 0.2f, Quaternion.identity);
            Instantiate(seperate_obj, transform.position + Vector3.left * 0.2f, Quaternion.identity);
        }

    }

}