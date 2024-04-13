using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SHS
{
    public enum EnemyType
    {
        Follower,
        Jumper,
        Sniper
    }

    public class EnemySpawner_v2 : MonoBehaviour
    {
        public static EnemySpawner_v2 Instance;

        //필요변수
        Transform player_trns;

        private void Awake()
        {
            Instance = this;

            Initialize(20);
        }


        void Start()
        {
            //플레이어 Transform 불러오기
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Invoke( "Start", 1f);
                return;
            }
            player_trns = GameObject.FindGameObjectWithTag("Player").transform;

        }

        void Update()
        {
            if (HandlingStart)
            {
                HandlingStart = false;
                StartCoroutine(EnemySpawn_Coroutine(now_enemytype, wave_scale, spawn_cooltime_set, spawn_radius));
            }
        }

        #region 소환 설정

        [Header("수동조작")]
        [SerializeField] bool HandlingStart;

        [Header("스폰 설정")]
        [SerializeField] EnemyType now_enemytype;
        [SerializeField] int wave_scale;
        [SerializeField] Vector2 spawn_cooltime_set = new Vector2(1f, 5f);
        [SerializeField] Vector2 spawn_radius = new Vector2(5f, 20f);

        public void Set_SpawnSetting(EnemyType _type, int scale, Vector2 cooltime, Vector2 radius)
        {
            now_enemytype = _type;
            wave_scale = scale;
            spawn_cooltime_set = cooltime;
            spawn_radius = radius;
        }

        #endregion

        private void Initialize(int initCount)
        {
            for (int i = 0; i < initCount; i++)
            {
                EnemyQueue.Enqueue(CreateNewEnemy());
                JumpEnemyQueue.Enqueue(CreateNewJumpEnemy());
                SnipeEnemyQueue.Enqueue(CreateNewSnipeEnemy());
            }
        }

        public static void ReturnObject(Enemy _enemy)
        {
            _enemy.gameObject.SetActive(false);
            _enemy.transform.SetParent(Instance.transform);

            switch (_enemy.Get_MyStat().enemy_id)
            {
                default:
                    Debug.LogError("id가 할당되지 않은 적개체입니다.");
                    Destroy(_enemy.gameObject);
                    break;

                //follower
                case 0:
                    Instance.EnemyQueue.Enqueue(_enemy);
                    break;

                //Jumper
                case 8:
                    Instance.JumpEnemyQueue.Enqueue(_enemy);
                    break;

                //Square
                case 11:
                    Destroy(_enemy.gameObject);
                    break;

            }

        }

        #region 적군 종류

        #region 기본 적군 풀링

        [Header("Follower 풀링")]
        [SerializeField] GameObject Enemy_prefab;

        [SerializeField] Queue<Enemy> EnemyQueue = new Queue<Enemy>();

        Enemy CreateNewEnemy()
        {
            var newObj = Instantiate(Enemy_prefab).GetComponent<Enemy>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        public static Enemy GetEnemy()
        {
            if (Instance.EnemyQueue.Count > 0)
            {
                var obj = Instance.EnemyQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var newObj = Instance.CreateNewEnemy();
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        #endregion

        #region 점핑 적군 풀링

        [Header("Jumper 풀링")]
        [SerializeField] GameObject JumpEnemy_prefab;

        [SerializeField] Queue<Enemy> JumpEnemyQueue = new Queue<Enemy>();

        Enemy CreateNewJumpEnemy()
        {
            var newObj = Instantiate(JumpEnemy_prefab).GetComponent<Enemy>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        public static Enemy GetJumpEnemy()
        {
            if (Instance.EnemyQueue.Count > 0)
            {
                var obj = Instance.JumpEnemyQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var newObj = Instance.CreateNewJumpEnemy();
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        #endregion

        #region 저격 적군 풀링

        [Header("Sniper 풀링")]
        [SerializeField] GameObject SnipeEnemy_prefab;

        [SerializeField] Queue<Enemy> SnipeEnemyQueue = new Queue<Enemy>();

        Enemy CreateNewSnipeEnemy()
        {
            var newObj = Instantiate(SnipeEnemy_prefab).GetComponent<Enemy>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        public static Enemy GetSnipeEnemy()
        {
            if (Instance.EnemyQueue.Count > 0)
            {
                var obj = Instance.SnipeEnemyQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var newObj = Instance.CreateNewSnipeEnemy();
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        #endregion

        #endregion 

        #region 쿨타임 생성

        IEnumerator EnemySpawn_Coroutine(EnemyType enemytype, int scale, Vector2 cooltime, Vector2 radius)
        {
            int a = 0;

            var newEnemy = new Enemy();


            while (a < scale)
            {
                //유저를 중심으로 spawn_radius거리에 랜덤으로 생성
                Vector2 randomPosition = Random.insideUnitCircle;
                Vector3 ranpos_v3 = new Vector3(randomPosition.x, randomPosition.y, 0).normalized;

                switch (enemytype)
                {
                    default:
                        Debug.LogError("해당하는 적의 타입이 없습니다. 생성종료");
                        yield break;

                    case EnemyType.Follower:
                        newEnemy = GetEnemy();
                        break;

                    case EnemyType.Jumper:
                        newEnemy = GetJumpEnemy();
                        break;

                    case EnemyType.Sniper:
                        newEnemy = GetSnipeEnemy();
                        break;
                }


                newEnemy.transform.position = player_trns.transform.position + ranpos_v3 * Random.Range(radius.x, radius.y);
                //newEnemy.transform.position = UnitManager.Instance.player.transform.position + ranpos_v3 * Random.Range(spawn_radius_ran.x, spawn_radius_ran.y);

                //UnitManager.Instance.enemies.Enqueue(newEnemy);

                newEnemy.GetComponent<Enemy>().Start_Burrowing();

                a++;

                //스폰 쿨타임 세팅
                yield return new WaitForSeconds(Random.Range(cooltime.x, cooltime.y));
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
}