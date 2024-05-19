using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Jumper : MonoBehaviour
    {
        //변수
        EnemyStat m_stat;
        Transform player_trns;
        Enemy m_enemy;
        Animator m_ani;

        // Start is called before the first frame update
        void Start()
        {
            m_enemy = GetComponent<Enemy>();
            m_stat = GetComponent<Enemy>().Get_MyStat();
            player_trns = GameObject.FindGameObjectWithTag("Player").transform;
            m_ani = GetComponent<Animator>();
        }

        [SerializeField] bool now_active;
        // Update is called once per frame
        void Update()
        {
            if (m_enemy.now_burrow && now_active)
            {
                now_active = false;
                m_ani.SetTrigger("end");
            }
            else if(!m_enemy.now_burrow && !now_active)
            {
                now_active = true;
                m_ani.SetTrigger("goon");
            }

            Move();
        }

        //플레이어로 향하는 방향
        Vector2 Direction()
        {
            Vector2 dir = player_trns.position - transform.position ;

            return dir.normalized;
        }

        [Header("소리")]
        [SerializeField] AudioSource jump_audio;
        [SerializeField] AudioSource land_audio;

        void Ani_Jump()
        {
            jump_audio.Play();
        }

        [SerializeField] ParticleSystem ptc_dusty;
        void Ani_Dusty()
        {
            ptc_dusty.Play();
            land_audio.Play();
        }


        [SerializeField] float ani_speed;

        Vector2 save_dir;
        // 움직임
        void Move()
        {
            if (m_enemy.now_burrow)
                return;

            if (!ptc_dusty.gameObject.activeSelf)
            {
                ptc_dusty.gameObject.SetActive(true);
            }

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