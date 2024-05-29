using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SHS
{
    public class EnemySpawner_v3 : MonoBehaviour
    {
        public static EnemySpawner_v3 Instance;

        //인스턴스
        EnemyQueueManager enemyqueue;
        SpecialEnemySpawner sp_enemyspawn;

        //필요변수
        Transform player_trns;

        private void Awake()
        {
            Instance = this;
        }


        void Start()
        {
            enemyqueue = EnemyQueueManager.instance;
            sp_enemyspawn = SpecialEnemySpawner.Instance;

            //플레이어 Transform 불러오기
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

                //5초마다 생성
                StartCoroutine(WaveMaker());
            }
        }

        IEnumerator WaveMaker()
        {
            wave_count = 0;

            int r_value = Random.Range(1, 10);

            while (GameManager.Instance.Timer > 5)
            {
                wave_count++;

                EnemyGroup[] set_eg;

                //웨이브 종류 할당
                switch (wave_level)
                {
                    default:
                    case 1:
                        set_eg = wave1_eg;

                        //테스트용
                        if (wave_count == 1)
                        {
                            Debug.Log("보스 생성");
                            sp_enemyspawn.BossSpawn(101);
                        }
                        //

                        break;

                    case 2:
                    case 3:
                        set_eg = enemyGroupList[Random.Range(0, enemyGroupList.Count)].enemyGroup;
                        break;

                    case 4:
                        set_eg = hardEnemyGroupList[Random.Range(0, hardEnemyGroupList.Count)].enemyGroup;
                        break;

                    case 5:
                        set_eg = hardEnemyGroupList[Random.Range(0, hardEnemyGroupList.Count)].enemyGroup;

                        if (wave_count == 1)
                        {
                            Debug.Log("보스 생성");
                            sp_enemyspawn.BossSpawn(101);
                        }

                        break;
                }

                //적 생성
                for (int i = 0; i < set_eg.Length; i++)
                {
                    StartCoroutine(EnemySpawn_Coroutine(set_eg[i]));
                }


                //5초마다 생성
                yield return new WaitForSeconds(5f);
            }
            Debug.Log($"{wave_level} 웨이브 종료");
            wave_level++;

        }

        [Header("웨이브 설정")]
        [SerializeField] int wave_level;
        [SerializeField] int wave_count;
        [SerializeField] int wave_maxcount { get => 3 + GameManager.Instance.Stage * 2 - 1; }

        [Header("Part 1")]  //하급몬스터1(1/5/10), 30초후 엘리트몬스터1 (150/5/1)
        [SerializeField] EnemyGroup[] wave1_eg;

        [Header("Part 2")]
        [SerializeField] EnemyGroup[] wave2_eg;

        [Header("Part Random")]
        [SerializeField] EnemyGroup[] wave_r_1;
        [SerializeField] EnemyGroup[] wave_r_2;
        [SerializeField] EnemyGroup[] wave_r_3;
        [SerializeField] EnemyGroup[] wave_r_4;
        [SerializeField] EnemyGroup[] wave_r_5;
        [SerializeField] EnemyGroup[] wave_r_6;
        [SerializeField] EnemyGroup[] wave_r_7;
        [SerializeField] EnemyGroup[] wave_r_8;
        [SerializeField] EnemyGroup[] wave_r_9;
        [SerializeField] List<ListEnemyGroup> enemyGroupList;
        [SerializeField] List<ListEnemyGroup> hardEnemyGroupList;

        public int Get_WaveLevel()
        {
            return wave_level;
        }

        #region 소환 설정

        [Header("작동조작")]
        [SerializeField] bool WaveStart;
        [SerializeField] bool WaveStop;

        public void MakeWave()
        {
            WaveStart = true;
        }

        [Header("스폰 설정")]
        [SerializeField] Vector2 spawn_cooltime_set = new Vector2(1f, 5f);
        [SerializeField] Vector2 spawn_radius = new Vector2(5f, 18f);

        #endregion

        #region 쿨타임 생성

        IEnumerator EnemySpawn_Coroutine(EnemyGroup set_eg)
        {
            int a = 0;

            var newEnemy = new Enemy();

            while (a < set_eg.enemyscale)
            {
                //유저를 중심으로 spawn_radius거리에 랜덤으로 생성
                Vector2 randomPosition = Random.insideUnitCircle;
                Vector3 ranpos_v3 = new Vector3(randomPosition.x, randomPosition.y, 0).normalized;

                switch (set_eg.enemystat.enemytype)
                {
                    default:
                        Debug.LogError("해당하는 적의 타입이 없습니다. 생성종료");
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

                    case EnemyType.Seperater:
                        newEnemy = enemyqueue.GetSepertEnemy();
                        break;

                    case EnemyType.Shotgun:
                        newEnemy = enemyqueue.GetShotgunEnemy();
                        break;

                    case EnemyType.Skater:
                        newEnemy = enemyqueue.GetSkaterEnemy();
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

        #region 트리거 | 그룹생성

        [Header("그룹 생성")]
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

                        Debug.LogError("설정된 그룹 번호가 없습니다. 0번 생성");
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

    [System.Serializable]
    public class ListEnemyGroup
    {
        public EnemyGroup[] enemyGroup;
    }
}

