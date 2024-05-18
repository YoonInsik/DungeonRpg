using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.PlayerSettings;

namespace SHS
{
    //���� �⺻������ ������ ���� ��.
    public class Enemy : MonoBehaviour
    {

        //�ν��Ͻ�
        MeatMart meatmart;

        //����
        Collider2D m_col;

        [Header("��������")]
        [SerializeField] bool handling_start;
        [SerializeField] bool Burrow_Skip;

        [Header("�� ��ü ����")]
        public EnemyStat m_stat;

        [SerializeField] float now_hp;

        private void Start()
        {
            //�ν��Ͻ�
            meatmart = MeatMart.instance;

            //����
            m_col = GetComponent<Collider2D>();
            m_col.enabled = false;

            now_hp = m_stat.hp + m_stat.hp_scaling * EnemySpawner_v3.Instance.Get_WaveLevel();

            now_burrow = true;

            if (handling_start)
            {
                Start_Burrowing();
            }

            if (Burrow_Skip)
            {
                now_burrow = false;

                m_col.enabled = true;
            }
        }

        public void Reset_MyStat(EnemyStat _stat)
        {
            m_stat = _stat;

            now_hp = m_stat.hp + m_stat.hp_scaling * EnemySpawner_v3.Instance.Get_WaveLevel();
        }

        public EnemyStat Get_MyStat()
        {
            return m_stat;
        }

        [Header("����Ʈ")]
        [SerializeField] ParticleSystem ptc_damaged;
        [SerializeField] ParticleSystem ptc_dead;
        [SerializeField] GameObject damagescale;


        public void Damaged(float _damage)
        {
            if (now_burrow)
                return; 

            now_hp -= _damage;
            if (now_hp <= 0)
            {
                Dead();
                return;
            }
            GameObject ds = Instantiate(damagescale);
            ds.transform.position = transform.position;
            ds.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 8f + Vector2.right * Random.Range(-5f, 5f), ForceMode2D.Impulse);
            ds.GetComponent<TextMeshPro>().text = _damage.ToString();
            ptc_damaged.Play();
        }

        /* �۵� �ȵ�, ������Ʈ�� ��Ȱ��ȭ�Ǹ鼭 �����µ�, ���� �ı���Ŵ
        IEnumerator Destroy_DamageScale(GameObject _obj)
        {
            int a = 0;

            while(a < 1)
            {
                a ++;
                yield return new WaitForSeconds(1f);
            }

            Destroy(_obj);
        }
        */

        [Header("���")]
        public bool now_burrow;
        [SerializeField] ParticleSystem burrow_par;
        [SerializeField] GameObject burrow_image;
        [SerializeField] GameObject Model;

        public void Start_Burrowing()
        {
            burrow_par.Play();

            Invoke("Burrow_End", Random.Range(3f, 5f));
        }

        public void Dead_Resetting()
        {
            now_burrow = true;

            m_col.enabled = false;
            now_hp = m_stat.hp;
            burrow_image.SetActive(true);
            Model.SetActive(false);
        }

        void Burrow_End()
        {
            now_burrow = false;

            m_col.enabled = true;

            burrow_par.Stop();
            burrow_image.SetActive(false);
            Model.SetActive(true);
        }

        public float Get_NowHP()
        {
            return now_hp;
        }

        [Header("����")]
        [SerializeField] bool not_queue;

        [Header("�����")]
        [SerializeField] int meat_num;
        [Range(0, 100)]
        [SerializeField] float meatdrop_rate;


        void Dead()
        {

            if (GetComponent<EnemyAI_Seperater>() != null)
            {
                GetComponent<EnemyAI_Seperater>().Seperate();
            }

            Vector3 _dir = new Vector3( Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);

            //����ġ ���
            if (ObjectPoolManager.Instance != null)
            {
                var exp = ObjectPoolManager.Instance.GetGo("Exp");
                exp.transform.position = transform.position + _dir;
            }

            //��� ���
            if (Random.Range(0, 100) < meatdrop_rate)
            {
                GameObject target = Instantiate(MapManager.Instance.CurChunk.data.dropTable[0].item, transform.position, Quaternion.identity);

                target.transform.position = transform.position + _dir * -1f;
            }



            // if (Random.Range(0, 100) < meatdrop_rate)
            // {
            //     if (meatmart.Meats[meat_num] == null)
            //         Debug.LogError("�ش� ���� �������� �ʽ��ϴ�");
            //     else
            //         Instantiate(meatmart.Meats[meat_num], transform.position, Quaternion.identity);
            // }

            Instantiate(ptc_dead, transform.position, Quaternion.identity);

            if (not_queue)
            {
                Destroy(gameObject);
                return;
            }
            
            Dead_Resetting();

            EnemyDeadCount.instance.Counting();
            EnemyQueueManager.ReturnObject(this);
        
        }
        /*
        IEnumerator Drop_Move(GameObject _target, Vector3 _dir)
        {
            float a = 0.5f;

            while(a > 0)
            {
                a -= Time.deltaTime;

                _target.transform.Translate(_dir.normalized * Time.deltaTime);

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        */
    }
}