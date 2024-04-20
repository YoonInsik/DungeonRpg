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
            player_trns = UnitManager.Instance.player.transform;

            spawn_cooltime = spawn_cooltime_set;

            // StartCoroutine(EnemySpawn_Coroutine(spawn_cooltime));
        }

        private void Initialize(int initCount)
        {
            for (int i = 0; i < initCount; i++)
            {
                EnemyQueue.Enqueue(CreateNewEnemy());
            }
            for (int i = 0; i < initCount; i++)
            {
                JumpEnemyQueue.Enqueue(CreateNewJumpEnemy());
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

        #region 쿨타임 생성


        [Header("쿨타임 생성")]
        [SerializeField] float spawn_cooltime_set;
        [SerializeField] float spawn_radius = 25f;
        [SerializeField] Vector2 spawn_radius_ran = new Vector2(0.1f, 0.5f);

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
                GameObject e = GetEnemy().gameObject;
                e.transform.position = player_trns.position + ranpos_v3 * spawn_radius;

            }
        }

        #endregion

        [Space(10)]

        #region 점퍼생성

        public bool make_jumper;

        void JumperMaker()
        {
            if (make_jumper)
            {
                make_jumper = false;

                Vector2 randomPosition = Random.insideUnitCircle;
                Vector3 ranpos_v3 = new Vector3(randomPosition.x, randomPosition.y, 0).normalized;
                GameObject e = GetJumpEnemy().gameObject;
                e.transform.position = player_trns.position + ranpos_v3 * spawn_radius;
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

        #region 적 생성

        private float RandomFloat() {
            return UnityEngine.Random.Range(-MapManager.CHUNKSIZE / 2 + 1, MapManager.CHUNKSIZE / 2);
        }
        public IEnumerator EnemySpawn_Coroutine(float cooltime)
        {
            while(true)
            {
                //청크 기준으로 사각형 랜덤 생성
                Vector3 newPos = MapManager.Instance.CurChunk.transform.position + new Vector3(RandomFloat(), RandomFloat(), 0);

                var newEnemy = GetEnemy();
                newEnemy.transform.position = newPos;
                UnitManager.Instance.enemies.Enqueue(newEnemy);
                newEnemy.Start_Burrowing();

                yield return new WaitForSeconds(cooltime);
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