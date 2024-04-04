using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Jumper : MonoBehaviour
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

        [SerializeField] ParticleSystem ptc_dusty;
        void Ani_Dusty()
        {
            ptc_dusty.Play();
        }


        [SerializeField] float ani_speed;

        Vector2 save_dir;
        // ������
        void Move()
        {
            if (ani_speed <= 0)
            {
                save_dir = Direction();
            }
            else
            {
                transform.Translate(m_stat.speed * ani_speed * save_dir * Time.deltaTime);
            }
        }

    }

}