using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Passing : MonoBehaviour
    {
        //변수
        EnemyStat m_stat;
        Transform player_trns;

        [Header("움직임 설정")]
        [SerializeField] Vector2 setting_dir;

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

        //플레이어로 향하는 방향
        Vector2 Direction()
        {
            Vector2 dir = setting_dir;

            return dir.normalized;
        }

        // 움직임
        void Move()
        {
            transform.Translate(m_stat.speed * Direction() * Time.deltaTime);
        }
    }

}