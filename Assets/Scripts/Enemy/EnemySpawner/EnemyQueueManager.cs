using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SHS
{
    public class EnemyQueueManager : MonoBehaviour
    {
        public static EnemyQueueManager instance;

        private void Awake()
        {
            instance = this;

            Initialize(20);
        }


        void Start()
        {
            //플레이어 Transform 불러오기
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Debug.Log(this.name + " 재시작");
                Invoke( "Start", 1f);
                return;
            }
        }

        private void Initialize(int initCount)
        {
            for (int i = 0; i < initCount; i++)
            {
                EnemyQueue.Enqueue(CreateNewEnemy());
                JumpEnemyQueue.Enqueue(CreateNewJumpEnemy());
                SnipeEnemyQueue.Enqueue(CreateNewSnipeEnemy());
                SepertEnemyQueue.Enqueue(CreateNewSepertEnemy());
                ShotgunEnemyQueue.Enqueue(CreateNewShotgunEnemy());
            }
        }

        public void ClearMonster()
        {
            GameObject[] Activated_enemys = GameObject.FindGameObjectsWithTag("Enemy");

            for(int i = 0; i< Activated_enemys.Length; i++)
            {
                if (Activated_enemys[i].transform.parent == null)
                    ReturnObject(Activated_enemys[i].GetComponent<Enemy>());
            }

        }

        public static void ReturnObject(Enemy _enemy)
        {
            _enemy.gameObject.SetActive(false);
            _enemy.transform.SetParent(instance.transform);

            switch (_enemy.Get_MyStat().enemytype)
            {
                default:
                    Debug.LogError("id가 할당되지 않은 적개체입니다.");
                    Destroy(_enemy.gameObject);
                    break;

                //follower
                case EnemyType.Follower:
                    instance.EnemyQueue.Enqueue(_enemy);
                    break;

                //Jumper
                case EnemyType.Jumper:
                    instance.JumpEnemyQueue.Enqueue(_enemy);
                    break;

                //Sniper
                case EnemyType.Sniper:
                    instance.SnipeEnemyQueue.Enqueue(_enemy);
                    break;

                //Seperate
                case EnemyType.Seperater:
                    instance.SepertEnemyQueue.Enqueue(_enemy);
                    break;

                //Shotgun
                case EnemyType.Shotgun:
                    instance.ShotgunEnemyQueue.Enqueue(_enemy);
                    break;

                //Skater
                case EnemyType.Skater:
                    instance.SkaterEnemyQueue.Enqueue(_enemy);
                    break;
            }

        }

        #region 적군 종류

        #region 기본 적군 풀링

        [Header("Follower 풀링")]
        [SerializeField] GameObject Enemy_prefab;

        public Queue<Enemy> EnemyQueue = new Queue<Enemy>();

        Enemy CreateNewEnemy()
        {
            var newObj = Instantiate(Enemy_prefab).GetComponent<Enemy>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        public Enemy GetEnemy()
        {
            if (instance.EnemyQueue.Count > 0)
            {
                var obj = instance.EnemyQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var newObj = instance.CreateNewEnemy();
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        #endregion

        #region 점핑 적군 풀링

        [Header("Jumper 풀링")]
        [SerializeField] GameObject JumpEnemy_prefab;

        public Queue<Enemy> JumpEnemyQueue = new Queue<Enemy>();

        Enemy CreateNewJumpEnemy()
        {
            var newObj = Instantiate(JumpEnemy_prefab).GetComponent<Enemy>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        public Enemy GetJumpEnemy()
        {
            if (instance.EnemyQueue.Count > 0)
            {
                var obj = instance.JumpEnemyQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var newObj = instance.CreateNewJumpEnemy();
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        #endregion

        #region 저격 적군 풀링

        [Header("Sniper 풀링")]
        [SerializeField] GameObject SnipeEnemy_prefab;

        public Queue<Enemy> SnipeEnemyQueue = new Queue<Enemy>();

        Enemy CreateNewSnipeEnemy()
        {
            var newObj = Instantiate(SnipeEnemy_prefab).GetComponent<Enemy>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        public Enemy GetSnipeEnemy()
        {
            if (instance.EnemyQueue.Count > 0)
            {
                var obj = instance.SnipeEnemyQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var newObj = instance.CreateNewSnipeEnemy();
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        #endregion

        #region 분열 적군 풀링

        [Header("Seperater 풀링")]
        [SerializeField] GameObject SepertEnemy_prefab;

        public Queue<Enemy> SepertEnemyQueue = new Queue<Enemy>();

        Enemy CreateNewSepertEnemy()
        {
            var newObj = Instantiate(SepertEnemy_prefab).GetComponent<Enemy>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        public Enemy GetSepertEnemy()
        {
            if (instance.SepertEnemyQueue.Count > 0)
            {
                var obj = instance.SepertEnemyQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var newObj = instance.CreateNewSepertEnemy();
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        #endregion

        #region 샷건 적군 풀링

        [Header("Shotgun 풀링")]
        [SerializeField] GameObject ShotgunEnemy_prefab;

        public Queue<Enemy> ShotgunEnemyQueue = new Queue<Enemy>();

        Enemy CreateNewShotgunEnemy()
        {
            var newObj = Instantiate(ShotgunEnemy_prefab).GetComponent<Enemy>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        public Enemy GetShotgunEnemy()
        {
            if (instance.ShotgunEnemyQueue.Count > 0)
            {
                var obj = instance.ShotgunEnemyQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var newObj = instance.CreateNewShotgunEnemy();
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        #endregion

        #region 스케이팅 적군 풀링

        [Header("Shotgun 풀링")]
        [SerializeField] GameObject SkaterEnemy_prefab;

        public Queue<Enemy> SkaterEnemyQueue = new Queue<Enemy>();

        Enemy CreateNewSkaterEnemy()
        {
            var newObj = Instantiate(SkaterEnemy_prefab).GetComponent<Enemy>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        public Enemy GetSkaterEnemy()
        {
            if (instance.SkaterEnemyQueue.Count > 0)
            {
                var obj = instance.SkaterEnemyQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var newObj = instance.CreateNewSkaterEnemy();
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        #endregion

        #endregion 

    }
}