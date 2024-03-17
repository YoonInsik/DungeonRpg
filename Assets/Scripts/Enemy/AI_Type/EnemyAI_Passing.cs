using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Passing : MonoBehaviour
    {
        //����
        EnemyStat m_stat;
        Transform player_trns;

        [Header("������ ����")]
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

        //�÷��̾�� ���ϴ� ����
        Vector2 Direction()
        {
            Vector2 dir = setting_dir;

            return dir.normalized;
        }

        // ������
        void Move()
        {
            transform.Translate(m_stat.speed * Direction() * Time.deltaTime);
        }
    }

}