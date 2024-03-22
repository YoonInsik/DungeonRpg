using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemySpawner : MonoBehaviour
    {
        //�ʿ亯��
        Transform player_trns;

        // Start is called before the first frame update
        void Start()
        {
            player_trns = GameObject.FindGameObjectWithTag("Player").transform;

            spawn_cooltime = spawn_cooltime_set;
        }

        // Update is called once per frame
        void Update()
        {
            //�� ���� ���� _ ��Ÿ��
            EnemySpawn_Cooltime();

            //�� ���� ���� _ Ʈ���� | �׷�
            EnemySpawn_Group();

        }

        #region ��Ÿ�� ����


        [Header("��Ÿ�� ����")]
        [SerializeField] GameObject[] Enemy_prefab;
        [SerializeField] float spawn_cooltime_set;
        [SerializeField] float spawn_radius = 25f;

        [Header("��Ÿ�� ����_��ġȮ��")]
        [SerializeField] float spawn_cooltime;


        //�� ���� ���� _ ��Ÿ��
        void EnemySpawn_Cooltime()
        {
            if(spawn_cooltime > 0)
            {
                spawn_cooltime -= Time.deltaTime;
                return;
            }
            else
            {
                spawn_cooltime = spawn_cooltime_set;

                //������ �߽����� spawn_radius�Ÿ��� �������� ����
                Vector2 randomPosition = Random.insideUnitCircle;
                Vector3 ranpos_v3 = new Vector3(randomPosition.x, randomPosition.y, 0).normalized;
                Instantiate(Enemy_prefab[Random.Range(0, Enemy_prefab.Length)], player_trns.position + ranpos_v3 * spawn_radius, Quaternion.identity);
            }
        }

        #endregion

        [Space(10)]

        #region Ʈ���� | �׷����

        [Header("�׷� ����")]
        [SerializeField] GameObject[] Enemy_Group;
        [SerializeField] int group_num;
        [SerializeField] bool group_make;

        public void EnemySpawn_GroupTrigger()
        {
            group_make = true;
        }

        void EnemySpawn_Group()
        {
            if (group_make)
            {
                group_make = false;

                switch (group_num)
                {
                    default:

                        Debug.LogError("������ �׷� ��ȣ�� �����ϴ�. 0�� ����");
                        Instantiate(Enemy_Group[0], player_trns.position + Vector3.right * 30f, Quaternion.identity);

                        break;

                    case 0:

                        Instantiate(Enemy_Group[0], player_trns.position + Vector3.right * 30f, Quaternion.identity);

                        break;
                }


            }
        }

        #endregion


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, spawn_radius);

        }
    }
}