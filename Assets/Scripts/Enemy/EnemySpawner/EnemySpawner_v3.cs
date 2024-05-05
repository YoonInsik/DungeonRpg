using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace SHS
{
    public class EnemySpawner_v3 : MonoBehaviour
    {
        public static EnemySpawner_v3 Instance;

        //�ν��Ͻ�
        EnemyQueueManager enemyqueue;
        SpecialEnemySpawner sp_enemyspawn;

        //�ʿ亯��
        Transform player_trns;

        private void Awake()
        {
            Instance = this;
        }


        void Start()
        {
            enemyqueue = EnemyQueueManager.instance;
            sp_enemyspawn = SpecialEnemySpawner.Instance;

            //�÷��̾� Transform �ҷ�����
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Invoke("Start", 1f);
                return;
            }
            player_trns = GameObject.FindGameObjectWithTag("Player").transform;

            wave_level = 1;

        }

        void Update()
        {
            if (WaveStart)
            {
                WaveStart = false;

                //5�ʸ��� ����
                StartCoroutine(WaveMaker());
            }
        }

        IEnumerator WaveMaker()
        {
            wave_count = 0;

            while (wave_count < wave_maxcount)
            {
                wave_count++;

                EnemyGroup[] set_eg;

                //���̺� ���� �Ҵ�
                switch (wave_level)
                {
                    default:

                        Debug.LogError("�Ҵ�� ���̺갡 �����ϴ�.");

                        yield break;

                    case 1:

                        set_eg = wave1_eg;

                        if (wave_count == 6)
                        {
                            Debug.Log("���� ����");
                            sp_enemyspawn.BossSpawn(101);
                        }

                        break;

                    case 2:

                        set_eg = wave2_eg;

                        break;
                }

                //�� ����
                for (int i = 0; i < set_eg.Length; i++)
                {
                    StartCoroutine(EnemySpawn_Coroutine(set_eg[i]));
                }


                //5�ʸ��� ����
                yield return new WaitForSeconds(5f);
            }

            wave_level++;

        }

        [Header("���̺� ����")]
        [SerializeField] int wave_level;
        [SerializeField] int wave_count;
        [SerializeField] int wave_maxcount = 12;

        [Header("Part 1")]  //�ϱ޸���1(1/5/10), 30���� ����Ʈ����1 (150/5/1)
        [SerializeField] EnemyGroup[] wave1_eg;

        [Header("Part 2")]
        [SerializeField] EnemyGroup[] wave2_eg;




        #region ��ȯ ����

        [Header("�۵�����")]
        [SerializeField] bool WaveStart;
        [SerializeField] bool WaveStop;

        public void MakeWave()
        {
            WaveStart = true;
        }

        [Header("���� ����")]
        [SerializeField] Vector2 spawn_cooltime_set = new Vector2(1f, 5f);
        [SerializeField] Vector2 spawn_radius = new Vector2(5f, 20f);

        #endregion

        #region ��Ÿ�� ����

        IEnumerator EnemySpawn_Coroutine(EnemyGroup set_eg)
        {
            int a = 0;

            var newEnemy = new Enemy();

            while (a < set_eg.enemyscale)
            {
                //������ �߽����� spawn_radius�Ÿ��� �������� ����
                Vector2 randomPosition = Random.insideUnitCircle;
                Vector3 ranpos_v3 = new Vector3(randomPosition.x, randomPosition.y, 0).normalized;

                switch (set_eg.enemystat.enemytype)
                {
                    default:
                        Debug.LogError("�ش��ϴ� ���� Ÿ���� �����ϴ�. ��������");
                        yield break;

                    case EnemyType.Follower:
                        newEnemy = enemyqueue.GetEnemy();
                        break;

                    case EnemyType.Jumper:
                        newEnemy = enemyqueue.GetJumpEnemy();
                        break;

                    case EnemyType.Sniper:
                        newEnemy = enemyqueue.GetSnipeEnemy();
                        break;
                }

                newEnemy.Reset_MyStat(set_eg.enemystat);

                /*
                if (set_eg.use_customradius)
                {
                    newEnemy.transform.position = player_trns.transform.position + ranpos_v3 * Random.Range(set_eg.customradius.x, set_eg.customradius.y);
                }
                else
                    newEnemy.transform.position = player_trns.transform.position + ranpos_v3 * Random.Range(spawn_radius.x, spawn_radius.y);
                */

                if (set_eg.use_customradius)
                {
                    newEnemy.transform.position = MapManager.Instance.CurChunk.transform.position + ranpos_v3 * Random.Range(set_eg.customradius.x, set_eg.customradius.y);
                }
                else
                    newEnemy.transform.position = MapManager.Instance.CurChunk.transform.position + ranpos_v3 * Random.Range(spawn_radius.x, spawn_radius.y);

                //newEnemy.transform.position = UnitManager.Instance.player.transform.position + ranpos_v3 * Random.Range(spawn_radius_ran.x, spawn_radius_ran.y);

                //UnitManager.Instance.enemies.Enqueue(newEnemy);

                newEnemy.GetComponent<Enemy>().Start_Burrowing();

                a++;

                yield return new Null();
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
            Gizmos.DrawWireSphere(transform.position, spawn_radius.x);

            Gizmos.color = new Color(1f, 1f, 0f);
            Gizmos.DrawWireSphere(transform.position, spawn_radius.y);

        }
    }
}