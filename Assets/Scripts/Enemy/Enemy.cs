using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace SHS
{
    //���� �⺻������ ������ ���� ��.
    public class Enemy : MonoBehaviour
    {

        //�ν��Ͻ�
        MeatMart meatmart;

        [Header("�� ��ü ����")]
        [SerializeField] EnemyStat m_stat;

        [SerializeField] float now_hp;

        private void Start()
        {
            meatmart = MeatMart.instance;

            now_hp = m_stat.hp;

            now_burrow = true;

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

            burrow_image.SetActive(true);
            Model.SetActive(false);
        }

        void Burrow_End()
        {
            now_burrow = false;

            burrow_par.Stop();
            burrow_image.SetActive(false);
            Model.SetActive(true);
        }


        [Header("����")]

        [Header("�����")]
        [SerializeField] int meat_num;
        [Range(0, 100)]
        [SerializeField] float meatdrop_rate;


        void Dead()
        {
            //����ġ ���
            var exp = ObjectPoolManager.Instance.GetGo("Exp");
            exp.transform.position = transform.position;

            //��� ���
            if (Random.Range(0, 100) < meatdrop_rate)
            {
                Instantiate(MapManager.Instance.CurChunk.data.dropTable[0].item, transform.position, Quaternion.identity);
            }

            // if (Random.Range(0, 100) < meatdrop_rate)
            // {
            //     if (meatmart.Meats[meat_num] == null)
            //         Debug.LogError("�ش� ���� �������� �ʽ��ϴ�");
            //     else
            //         Instantiate(meatmart.Meats[meat_num], transform.position, Quaternion.identity);
            // }

            Instantiate(ptc_dead, transform.position, Quaternion.identity);

            Dead_Resetting();

            EnemySpawner.ReturnObject(this);
        
        }
    }
}