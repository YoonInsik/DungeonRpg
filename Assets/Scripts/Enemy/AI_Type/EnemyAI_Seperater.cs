using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Seperater : MonoBehaviour
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

        // 움직임
        void Move()
        {
            m_rigid.velocity = m_stat.speed * Direction();

        }

        [Header("분열")]
        [SerializeField] GameObject seperate_obj;

        public void Seperate()
        {
            Instantiate(seperate_obj, transform.position + Vector3.right * 0.2f, Quaternion.identity);
            Instantiate(seperate_obj, transform.position + Vector3.left * 0.2f, Quaternion.identity);
        }

    }

}