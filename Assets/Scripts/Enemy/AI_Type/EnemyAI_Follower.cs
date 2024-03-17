using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Follower : MonoBehaviour
    {
        //����
        EnemyStat m_stat;
        Transform player_trns;

        // Start is called before the first frame update
        void Start()
        {
            m_stat = GetComponent<Enemy>().Get_MyStat();
            player_trns = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {
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
            transform.Translate(m_stat.speed * Direction() * Time.deltaTime);
        }
    }

}