using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace SHS
{
    public class SpecialEnemySpawner : MonoBehaviour
    {
        public static SpecialEnemySpawner Instance;

        //�ν��Ͻ�
        EnemyQueueManager enemyqueue;

        //�ʿ亯��
        Transform player_trns;

        private void Awake()
        {
            Instance = this;
        }


        void Start()
        {
            enemyqueue = EnemyQueueManager.instance;

            //�÷��̾� Transform �ҷ�����
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Invoke( "Start", 1f);
                return;
            }
            player_trns = GameObject.FindGameObjectWithTag("Player").transform;

        }

        private void Update()
        {
            if (HandlingSpawn)
            {
                HandlingSpawn = false;
                BossSpawn(now_bossnum);
            }
        }

        #region ��ȯ

        [SerializeField] int now_bossnum;

        [Header("��������")]
        [SerializeField] bool HandlingSpawn;

        [Header("��������")]
        [SerializeField] GameObject boss_101;

        [Header("���� ����")]
        [SerializeField] Vector2 spawn_radius = new Vector2(5f, 20f);

        public void BossSpawn(int boss_num, float time)
        {
            Invoke("BossSpawn", time);
        }

        public void BossSpawn(int boss_num)
        {
            now_bossnum = boss_num;

            GameObject _boss;

            switch (now_bossnum)
            {
                default: Debug.LogError("�Ҵ�� ������ �����ϴ�.");  return;

                case 101:

                    _boss = boss_101;

                    break;
            }

            Vector2 randomPosition = Random.insideUnitCircle;
            Vector3 ranpos_v3 = new Vector3(randomPosition.x, randomPosition.y, 0).normalized;

            Instantiate(_boss, player_trns.transform.position + ranpos_v3 * Random.Range(spawn_radius.x, spawn_radius.y), Quaternion.identity).GetComponent<Enemy>().Start_Burrowing(); ;

        }

        public void BossSpawn()
        {
            GameObject _boss;

            switch (now_bossnum)
            {
                default: Debug.LogError("�Ҵ�� ������ �����ϴ�."); return;

                case 101:

                    _boss = boss_101;

                    break;
            }

            Vector2 randomPosition = Random.insideUnitCircle;
            Vector3 ranpos_v3 = new Vector3(randomPosition.x, randomPosition.y, 0).normalized;

            Instantiate(_boss, player_trns.transform.position + ranpos_v3 * Random.Range(spawn_radius.x, spawn_radius.y), Quaternion.identity).GetComponent<Enemy>().Start_Burrowing(); ;

        }

        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, spawn_radius.x);

            Gizmos.color = new Color(1f, 1f, 0f);
            Gizmos.DrawWireSphere(transform.position, spawn_radius.y);

        }
    }
}