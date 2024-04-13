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

        //�ʿ亯��
        Transform player_trns;

        private void Awake()
        {
            Instance = this;

            Initialize(20);
        }


        void Start()
        {
            //�÷��̾� Transform �ҷ�����
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

        #region ��ȯ ����

        [Header("��������")]
        [SerializeField] bool HandlingStart;

        [Header("���� ����")]
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

        #region ���� ����

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

        #region ���� ���� Ǯ��

        [Header("Sniper Ǯ��")]
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

        #region ��Ÿ�� ����

        IEnumerator EnemySpawn_Coroutine(EnemyType enemytype, int scale, Vector2 cooltime, Vector2 radius)
        {
            int a = 0;

            var newEnemy = new Enemy();


            while (a < scale)
            {
                //������ �߽����� spawn_radius�Ÿ��� �������� ����
                Vector2 randomPosition = Random.insideUnitCircle;
                Vector3 ranpos_v3 = new Vector3(randomPosition.x, randomPosition.y, 0).normalized;

                switch (enemytype)
                {
                    default:
                        Debug.LogError("�ش��ϴ� ���� Ÿ���� �����ϴ�. ��������");
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

                //���� ��Ÿ�� ����
                yield return new WaitForSeconds(Random.Range(cooltime.x, cooltime.y));
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