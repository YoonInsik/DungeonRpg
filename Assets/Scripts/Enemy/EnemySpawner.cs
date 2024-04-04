using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SHS
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance;

        //필요변수
        Transform player_trns;

        private void Awake()
        {
            Instance = this;

            Initialize(20);
        }


        // Start is called before the first frame update
        void Start()
        {
            player_trns = GameObject.FindGameObjectWithTag("Player").transform;

            spawn_cooltime = spawn_cooltime_set;
        }

        // Update is called once per frame
        void Update()
        {
            //적 생성 조건 _ 쿨타임
            //EnemySpawn_Cooltime();

            //적 생성 조건 _ 트리거 | 그룹
            //EnemySpawn_Group();

        }

        #region 풀링

        [Header("풀링")]

        Queue<Enemy> EnemyQueue = new Queue<Enemy>();

        private void Initialize(int initCount)
        {
            for (int i = 0; i < initCount; i++)
            {
                EnemyQueue.Enqueue(CreateNewEnemy());
            }
        }

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

        public static void ReturnObject(Enemy _enemy)
        {
            _enemy.gameObject.SetActive(false);
            _enemy.transform.SetParent(Instance.transform);
            Instance.EnemyQueue.Enqueue(_enemy);
        }

        #endregion

        #region 쿨타임 생성


        [Header("쿨타임 생성")]
        [SerializeField] GameObject Enemy_prefab;
        [SerializeField] float spawn_cooltime_set;
        [SerializeField] float spawn_radius = 25f;

        [Header("쿨타임 생성_수치확인")]
        [SerializeField] float spawn_cooltime;


        //적 생성 조건 _ 쿨타임
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

                //유저를 중심으로 spawn_radius거리에 랜덤으로 생성
                Vector2 randomPosition = Random.insideUnitCircle;
                Vector3 ranpos_v3 = new Vector3(randomPosition.x, randomPosition.y, 0).normalized;
                Instantiate(GetEnemy(), player_trns.position + ranpos_v3 * spawn_radius, Quaternion.identity);
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

        public IEnumerator EnemySpawn_Coroutine(float cooltime)
        {
            while(true)
            {
                //유저를 중심으로 spawn_radius거리에 랜덤으로 생성
                Vector2 randomPosition = Random.insideUnitCircle;
                Vector3 ranpos_v3 = new Vector3(randomPosition.x, randomPosition.y, 0).normalized;
                var newEnemy = GetEnemy();
                newEnemy.transform.position = UnitManager.Instance.player.transform.position + ranpos_v3 * spawn_radius;
                UnitManager.Instance.enemies.Enqueue(newEnemy);

                yield return new WaitForSeconds(cooltime);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, spawn_radius);

        }
    }
}