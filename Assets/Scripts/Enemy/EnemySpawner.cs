using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SHS
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance;

        //�ʿ亯��
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
                    Debug.LogError("id�� �Ҵ���� ���� ����ü�Դϴ�.");
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

        #region �⺻ ���� Ǯ��

        [Header("Follower Ǯ��")]
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

        #region ���� ���� Ǯ��

        [Header("Jumper Ǯ��")]
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

        #region ��Ÿ�� ����


        [Header("��Ÿ�� ����")]
        [SerializeField] float spawn_cooltime_set;
        [SerializeField] float spawn_radius = 25f;
        [SerializeField] Vector2 spawn_radius_ran = new Vector2(0.1f, 0.5f);

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
                GameObject e = GetEnemy().gameObject;
                e.transform.position = player_trns.position + ranpos_v3 * spawn_radius;

            }
        }

        #endregion

        [Space(10)]

        #region ���ۻ���

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

        #region �� ����

        private float RandomFloat() {
            return UnityEngine.Random.Range(-MapManager.CHUNKSIZE / 2 + 1, MapManager.CHUNKSIZE / 2);
        }
        public IEnumerator EnemySpawn_Coroutine(float cooltime)
        {
            while(true)
            {
                //ûũ �������� �簢�� ���� ����
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