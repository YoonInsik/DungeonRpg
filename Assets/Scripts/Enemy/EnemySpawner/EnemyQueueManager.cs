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

        [SerializeField] Queue<Enemy> JumpEnemyQueue = new Queue<Enemy>();

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

        [SerializeField] Queue<Enemy> SnipeEnemyQueue = new Queue<Enemy>();

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

        #endregion 

    }
}